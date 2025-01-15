using Quiz.Master.Core.Models;

namespace Quiz.Master.Services.ContentManagement;

public abstract record MiniGameDto
{
    public abstract MiniGameDefinitionData MapToDefinition();
    public abstract MiniGameType GameType { get; }
}
