using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using Quiz.Storage.Models;
using Quiz.Master.Core.Models;

namespace Quiz.Storage;

public class FileStorage: IFileStorage
{
    private readonly GridFSBucket _bucket;
    private readonly IMongoDatabase _database;
    private IMongoCollection<GameConfiguration> _configCollection => _database.GetCollection<GameConfiguration>("GameConfiguration");

    public FileStorage(IMongoDatabase database)
    {
        _bucket = new GridFSBucket(database);
        _database = database;
    }

    public async Task<string> UploadFileAsync(SimpleFileStream fileStream, CancellationToken cancellationToken = default)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "filename", fileStream.FileName },
                { "contentType", fileStream.ContentType }
            }
        };
        var objectId = await _bucket.UploadFromStreamAsync(fileStream.FileName, fileStream.FileStream, options, cancellationToken: cancellationToken);
        return objectId.ToString();
    }

    public async Task<SimpleFileStream> GetFileAsync(string? fileId, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrEmpty(fileId))
        {
            throw new ArgumentNullException(nameof(fileId));
        }

        var objectId = new ObjectId(fileId);
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
        var fileInfo = await _bucket.Find(filter).FirstOrDefaultAsync(cancellationToken);

        if (fileInfo == null)
        {
            throw new FileNotFoundException("File not found in GridFS.");
        }

        var fileName = fileInfo.Metadata.GetValue("filename").AsString;
        var contentType = fileInfo.Metadata.GetValue("contentType").AsString;

        var stream = new MemoryStream();
        await _bucket.DownloadToStreamAsync(objectId, stream, cancellationToken: cancellationToken);
        stream.Position = 0; // Reset stream position to the beginning

        return new (fileId, fileName, contentType, stream);
    }

    public async Task<IEnumerable<SimpleFile>> SearchFilesAsync(string? name = null, string? contentType = null, CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<GridFSFileInfo>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(name))
        {
            filter &= filterBuilder.Eq("metadata.filename", name);
        }

        if (!string.IsNullOrEmpty(contentType))
        {
            filter &= filterBuilder.Eq("metadata.contentType", contentType);
        }

        var cursor = await _bucket.FindAsync(filter, cancellationToken: cancellationToken);
        var fileInfos = await cursor.ToListAsync(cancellationToken);

        return fileInfos.Select(fileInfo 
            => new SimpleFile(
                fileInfo.Id.ToString(), 
                fileInfo.Metadata.GetValue("filename").AsString, 
                fileInfo.Metadata.GetValue("contentType").AsString));
    }

    public async Task<SimpleFileStream> GetBaseGameFileAsync(BaseGameFileType type, CancellationToken cancellationToken = default)
    {
        var configuration = await GetConfigAsync(cancellationToken);

        return type switch
        {
            BaseGameFileType.StartScreenImage => await GetFileAsync(configuration.StartScreenImageId, cancellationToken),
            BaseGameFileType.EndVideo => await GetFileAsync(configuration.EndVideoFileId, cancellationToken),
            BaseGameFileType.StartVideo => await GetFileAsync(configuration.StartVideoFileId, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid BaseGameFileType")
        };
    }

    private async Task<GameConfiguration> GetConfigAsync(CancellationToken cancellationToken = default)
    {
        var cursor = await _configCollection.FindAsync(Builders<GameConfiguration>.Filter.Empty, cancellationToken: cancellationToken);
        var configuration = await cursor.FirstOrDefaultAsync(cancellationToken);
        return configuration;
    }

    public async Task UpdateBaseGameFileAsync(SimpleFileStream fileStream, BaseGameFileType type, CancellationToken cancellationToken = default)
    {
        var configuration = await GetConfigAsync(cancellationToken) ?? new GameConfiguration();
        var fileId = await UploadFileAsync(fileStream, cancellationToken);
        var oldFileId = "";

        switch (type) {
            case BaseGameFileType.StartVideo:
                oldFileId = configuration.StartVideoFileId;
                configuration.StartVideoFileId = fileId;
                break;
            case BaseGameFileType.EndVideo:
                oldFileId = configuration.EndVideoFileId;
                configuration.EndVideoFileId = fileId;
                break;
            case BaseGameFileType.StartScreenImage:
                oldFileId = configuration.StartScreenImageId;
                configuration.StartScreenImageId = fileId;
                break;
        }

        if (!string.IsNullOrEmpty(oldFileId))
        {
            var oldObjectId = new ObjectId(oldFileId);
            await _bucket.DeleteAsync(oldObjectId, cancellationToken);
        }

        await _configCollection.ReplaceOneAsync(
            Builders<GameConfiguration>.Filter.Empty,
            configuration,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
    }
}