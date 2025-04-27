using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace KeyBoard.Services.FirebaseService
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly ILogger<FirebaseStorageService> _logger;

        public FirebaseStorageService(IConfiguration configuration, ILogger<FirebaseStorageService> logger)
        {
            _logger = logger;

            // Lấy đường dẫn tương đối từ appsettings.json
            var serviceAccountRelativePath = configuration["Firebase:ServiceAccountKeyPath"] ?? throw new ArgumentNullException("Firebase:ServiceAccountKeyPath is missing.");
            var bucketConfig = configuration["Firebase:Bucket"] ?? throw new ArgumentNullException("Firebase:Bucket is missing.");

            // Xử lý bucket name: loại bỏ tiền tố gs:// và hậu tố .firebasestorage.app
            _bucketName = bucketConfig
                .Replace("gs://", "") // Loại bỏ tiền tố gs://
                .Split(".firebasestorage.app")[0]; // Loại bỏ hậu tố .firebasestorage.app

            // Kết hợp đường dẫn tương đối với thư mục gốc của dự án
            var baseDirectory = AppContext.BaseDirectory;
            var projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            var serviceAccountPath = Path.Combine(projectDirectory, serviceAccountRelativePath);

            // Kiểm tra xem file có tồn tại không
            if (!File.Exists(serviceAccountPath))
            {
                _logger.LogError("Service account key file not found at: {ServiceAccountPath}", serviceAccountPath);
                throw new FileNotFoundException("Service account key file not found.", serviceAccountPath);
            }

            _logger.LogInformation("Service account key file found at: {ServiceAccountPath}", serviceAccountPath);

            // Khởi tạo FirebaseApp nếu chưa khởi tạo
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(serviceAccountPath)
                });
            }

            // Tạo StorageClient với thông tin xác thực
            var credential = GoogleCredential.FromFile(serviceAccountPath);
            _storageClient = StorageClient.Create(credential);
        }

        public async Task DeleteFileAsync(string objectName)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, objectName);
                _logger.LogInformation($"File {objectName} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting file: {ex.Message}");
                throw;
            }
        }

        public string GetFileUrl(string objectName)
        {
            return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            try
            {
                _logger.LogInformation("Starting file upload...");

                var fileName = Path.GetFileName(file.FileName);
                var filePath = $"{folder}/{fileName}";
                var stream = file.OpenReadStream();

                _logger.LogInformation($"Stream size: {stream.Length}");

                if (stream == null || stream.Length == 0)
                {
                    throw new Exception("File stream is empty.");
                }

                _logger.LogInformation($"Uploading file to Firebase storage: {filePath}");

                await _storageClient.UploadObjectAsync(_bucketName, filePath, file.ContentType, stream);

                var fileUrl = GetFileUrl(filePath);

                _logger.LogInformation("File uploaded successfully to Firebase storage.");

                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                throw;
            }
        }
    }
}