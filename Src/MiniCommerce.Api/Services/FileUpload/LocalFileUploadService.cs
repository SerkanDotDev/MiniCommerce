using Microsoft.AspNetCore.Hosting;
using MiniCommerce.Api.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MiniCommerce.Api.Services.FileUpload;

public class LocalFileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;

    public LocalFileUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            return null;

        var uploadsFolder = Path.Combine(_environment.WebRootPath, folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
    }

    public void DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return;

        var fullPath = Path.Combine(_environment.WebRootPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
