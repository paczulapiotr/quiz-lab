using Quiz.Master.Features.Game.CreateGame;
using static Quiz.Master.Services.ContentManagement.ContentManagementClient;

public interface IContentManagementClient
{
    Task<SimpleGameDefinition> GetGameDefinition(string indentifier, GameLanguage language);
    Task<IEnumerable<GameName>> GetGameNames();
}