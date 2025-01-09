using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using Quiz.Storage.Models;

namespace Quiz.Storage;

public class FileStorage: IFileStorage
{
    private readonly GridFSBucket _bucket;

    public FileStorage(IMongoDatabase database)
    {
        _bucket = new GridFSBucket(database);
    }

    public async Task<string> UploadFileAsync(SimpleFileStream fileStream)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "filename", fileStream.FileName },
                { "contentType", fileStream.ContentType }
            }
        };
        var objectId = await _bucket.UploadFromStreamAsync(fileStream.FileName, fileStream.FileStream, options);
        return objectId.ToString();
    }

    public async Task<SimpleFileStream> GetFileAsync(string fileId)
    {
        var objectId = new ObjectId(fileId);
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
        var fileInfo = await _bucket.Find(filter).FirstOrDefaultAsync();

        if (fileInfo == null)
        {
            throw new FileNotFoundException("File not found in GridFS.");
        }

        var fileName = fileInfo.Metadata.GetValue("filename").AsString;
        var contentType = fileInfo.Metadata.GetValue("contentType").AsString;

        var stream = new MemoryStream();
        await _bucket.DownloadToStreamAsync(objectId, stream);
        stream.Position = 0; // Reset stream position to the beginning

        return new (fileName, contentType, stream);
    }
}