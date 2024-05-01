namespace eStoreOnline.Application.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, string containerName);
}