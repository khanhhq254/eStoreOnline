using eStoreOnline.Application.Interfaces;

namespace eStoreOnline.Application.Implementations;

public class LocalFileStorageService : IStorageService
{
    public async Task<string> UploadFileAsync(Stream stream, string fileName, string containerName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", containerName, fileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        stream.Position = 0;
        await stream.CopyToAsync(fileStream);
        return Path.Combine("/images", containerName, fileName);
    }
}