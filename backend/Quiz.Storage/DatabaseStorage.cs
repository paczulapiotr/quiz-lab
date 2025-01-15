using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Quiz.Master.Core.Models;

namespace Quiz.Storage;

public class DatabaseStorage : IDatabaseStorage
{
    private readonly IMongoDatabase _database;

    public DatabaseStorage(IMongoDatabase mongoDatabase)
    {
        _database = mongoDatabase;
    }

    public static void RegisterClassMap()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonClassMap.RegisterClassMap(new GameClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameInstanceClassMap());
        BsonClassMap.RegisterClassMap(new GameDefinitionClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameDefinitionClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameDefinitionDataClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameStateDataClassMap());
    }


    public IMongoCollection<Game> Games => _database.GetCollection<Game>("Games");
    public IMongoCollection<GameDefinition> GameDefinitions => _database.GetCollection<GameDefinition>("GameDefinitions");
    public IMongoCollection<MiniGameInstance<TState>> MiniGameInstances<TState>() where TState: MiniGameStateData, new() 
        => _database.GetCollection<MiniGameInstance<TState>>("MiniGameInstances");
    public IMongoCollection<MiniGameDefinition<TDefinition>> MiniGameDefinitions<TDefinition>() where TDefinition : MiniGameDefinitionData, new()
        => _database.GetCollection<MiniGameDefinition<TDefinition>>("MiniGameDefinitions");

    public async Task<Game> FindGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        var result = await Games.FindAsync(x => x.Id == gameId, cancellationToken: cancellationToken);

        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        await Games.ReplaceOneAsync(x=>x.Id == game.Id, game, cancellationToken: cancellationToken);
    }

    public async Task InsertGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        await Games.InsertOneAsync(game, cancellationToken: cancellationToken);
    }

    public async Task UpdateMiniGameAsync<TState>(MiniGameInstance<TState> miniGame, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new()
    {
        await MiniGameInstances<TState>()
            .ReplaceOneAsync(
                x => x.Id == miniGame.Id, 
                miniGame, 
                cancellationToken: cancellationToken);
    }

    public async Task<MiniGameInstance<TState>> FindMiniGameAsync<TState>(Guid id, CancellationToken cancellationToken = default) where TState : MiniGameStateData, new()
    {
        var result = await MiniGameInstances<TState>().FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<GameDefinition> FindGameDefinitionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await GameDefinitions.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<MiniGameDefinition<TDefinition>> FindMiniGameDefinitionAsync<TDefinition>(Guid id, CancellationToken cancellationToken = default) where TDefinition : MiniGameDefinitionData, new()
    {
        var result = await MiniGameDefinitions<TDefinition>().FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    // TODO: Improve performance using aggregation
    public async Task<IEnumerable<(Player player, Dictionary<Guid, int> scores)>> ListPlayerScores(Guid gameId, CancellationToken cancellationToken = default)
    {
        var game = await FindGameAsync(gameId, cancellationToken);
        if (game == null)
        {
            return Enumerable.Empty<(Player player, Dictionary<Guid, int> scores)>();
        }

        var players = game.Players;

        var miniGames = MiniGameInstances<MiniGameStateData>().Find(x=>x.GameId == gameId).ToList();

        var scores = new Dictionary<Guid, Dictionary<Guid, int>>();
        foreach (var miniGame in miniGames)
        {
            foreach (var score in miniGame.PlayerScores) {
                if (scores.ContainsKey(score.PlayerId)) {
                    if(scores[score.PlayerId].ContainsKey(miniGame.Id)) {
                        scores[score.PlayerId][miniGame.Id] += score.Score;
                    } else {
                        scores[score.PlayerId].Add(miniGame.Id, score.Score);
                    }
                }
                else {
                    scores.Add(score.PlayerId, new Dictionary<Guid, int> {
                        { miniGame.Id, score.Score }
                    });
                }
            }
        }

        return scores.Select(x => (players.First(p => p.Id == x.Key), x.Value));
    }

    public async Task InsertMiniGameAsync<TState>(MiniGameInstance<TState> miniGame, CancellationToken cancellationToken = default)
    where TState : MiniGameStateData, new()
    {
        await MiniGameInstances<TState>()
            .InsertOneAsync(miniGame,
                cancellationToken: cancellationToken);
    }

    public async Task InsertGameDefinitionAsync(GameDefinition gameDefinition, CancellationToken cancellationToken = default)
    {
        await GameDefinitions
            .InsertOneAsync(gameDefinition, cancellationToken: cancellationToken);
    }

    public async Task InsertManyMiniGameDefinitionAsync(IEnumerable<MiniGameDefinition> gameDefinitions, CancellationToken cancellationToken = default)
    {
        await MiniGameDefinitions<MiniGameDefinitionData>()
            .InsertManyAsync(gameDefinitions, cancellationToken: cancellationToken);
    }
}