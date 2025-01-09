namespace Quiz.Storage.Models;

public record SimpleFileStream(string FileName, string ContentType, Stream FileStream);