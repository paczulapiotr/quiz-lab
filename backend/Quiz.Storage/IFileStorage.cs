using Quiz.Storage.Models;

namespace Quiz.Storage;

public interface IFileStorage {
    Task<SimpleFileStream> GetFileAsync(string fileId, CancellationToken cancellationToken = default);
    Task<string> UploadFileAsync(SimpleFileStream fileStream, CancellationToken cancellationToken = default);
    Task<IEnumerable<SimpleFile>> SearchFilesAsync(string? name = null, string? contentType = null, CancellationToken cancellationToken = default);
    Task<SimpleFileStream> GetBaseGameFileAsync(BaseGameFileType type, CancellationToken cancellationToken = default);
    Task UpdateBaseGameFileAsync(SimpleFileStream fileStream, BaseGameFileType type, CancellationToken cancellationToken = default);
}