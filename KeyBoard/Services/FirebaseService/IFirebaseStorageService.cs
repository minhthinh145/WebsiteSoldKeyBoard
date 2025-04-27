namespace KeyBoard.Services.FirebaseService
{
    public interface IFirebaseStorageService
    {
            Task<string> UploadFileAsync(IFormFile file, string folder);
            Task  DeleteFileAsync(string objectName);
            string GetFileUrl(string objectName);
    }
}
