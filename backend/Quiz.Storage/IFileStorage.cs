using Quiz.Storage.Models;

namespace Quiz.Storage;

public interface IFileStorage {
    Task<SimpleFileStream> GetFileAsync(string fileId);
    Task<string> UploadFileAsync(SimpleFileStream fileStream);
}