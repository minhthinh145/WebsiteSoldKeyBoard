using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KeyBoard.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using KeyBoard.Services.FirebaseService;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.Swagger.Annotations;

namespace YourProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly ILogger<FirebaseController> _logger;

        public FirebaseController(IFirebaseStorageService firebaseStorageService, ILogger<FirebaseController> logger)
        {
            _firebaseStorageService = firebaseStorageService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string folder)
        {
            try
            {
                _logger.LogInformation("Received upload request. File: {FileName}, Folder: {Folder}", file?.FileName, folder);

                if (string.IsNullOrEmpty(folder))
                {
                    _logger.LogWarning("Folder parameter is missing or empty.");
                    return BadRequest("Folder parameter is required.");
                }

                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file uploaded or file is empty.");
                    return BadRequest("No file uploaded.");
                }

                var fileUrl = await _firebaseStorageService.UploadFileAsync(file, folder);
                _logger.LogInformation("File uploaded successfully. FileUrl: {FileUrl}", fileUrl);
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {ErrorMessage}", ex.Message);
                return StatusCode(500, "Error uploading file.");
            }
        }

        /// <summary>
        /// Deletes a file from Firebase Storage
        /// </summary>
        /// <remarks>
        /// Deletes a file from Firebase Storage using the specified object name.
        /// </remarks>
        /// <param name="objectName">The name of the object to delete</param>
        /// <returns>Confirmation of deletion</returns>
        [HttpDelete("delete")]
        [SwaggerOperation(OperationId = "DeleteFile")]
        [SwaggerResponse(200, "File deleted successfully", typeof(object))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(500, "Server error")]
        public async Task<IActionResult> DeleteFile([FromQuery] string objectName)
        {
            try
            {
                if (string.IsNullOrEmpty(objectName))
                    return BadRequest("Object name is required.");

                var safeObjectName = SanitizePath(objectName);
                await _firebaseStorageService.DeleteFileAsync(safeObjectName);

                return Ok(new { Message = "File deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file from Firebase Storage. Object: {ObjectName}", objectName);
                return StatusCode(500, "An error occurred while deleting the file.");
            }
        }

        public class GetFileUrlResponse
        {
            public string FileUrl { get; set; }
        }

        /// <summary>
        /// Gets the URL of a file in Firebase Storage
        /// </summary>
        /// <remarks>
        /// Retrieves the public URL of a file in Firebase Storage using the specified object name.
        /// </remarks>
        /// <param name="objectName">The name of the object to retrieve</param>
        /// <returns>The URL of the file</returns>
        [HttpGet("get-url")]
        [SwaggerOperation(OperationId = "GetFileUrl")]
        [SwaggerResponse(200, "File URL retrieved successfully", typeof(GetFileUrlResponse))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(500, "Server error")]
        public IActionResult GetFileUrl([FromQuery] string objectName)
        {
            try
            {
                if (string.IsNullOrEmpty(objectName))
                    return BadRequest("Object name is required.");

                var safeObjectName = SanitizePath(objectName);
                var fileUrl = _firebaseStorageService.GetFileUrl(safeObjectName);

                return Ok(new GetFileUrlResponse { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving file URL from Firebase Storage. Object: {ObjectName}", objectName);
                return StatusCode(500, "An error occurred while retrieving the file URL.");
            }
        }

        private string SanitizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be empty.");
            return path.Replace("..", "").Replace("/", "").Replace("\\", "");
        }
    }
}