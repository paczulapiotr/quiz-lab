namespace Quiz.Storage.Models;

public record SimpleFileStream(string? Id, string FileName, string ContentType, Stream FileStream) : SimpleFile(Id, FileName, ContentType);