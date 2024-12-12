using Microsoft.AspNetCore.Http;

namespace MiniCommerce.Api.Services.FileUpload;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    void DeleteFile(string filePath);
}
