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
        BsonClassMap.RegisterClassMap(new GameDefinitionClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameDefinitionDataClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameStateDataClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameInstanceClassMap());
        BsonClassMap.RegisterClassMap(new MiniGameDefinitionClassMap());
        BsonClassMap.RegisterClassMap(new RoomClassMap());
    }


    public IMongoCollection<Game> Games => _database.GetCollection<Game>("Games");
    public IMongoCollection<Room> Rooms => _database.GetCollection<Room>("Rooms");
    public IMongoCollection<GameDefinition> GameDefinitions => _database.GetCollection<GameDefinition>("GameDefinitions");
    public IMongoCollection<MiniGameInstance> MiniGameInstances
        => _database.GetCollection<MiniGameInstance>("MiniGameInstances");
    public IMongoCollection<MiniGameDefinition> MiniGameDefinitions
        => _database.GetCollection<MiniGameDefinition>("MiniGameDefinitions");

    public async Task<Game> FindGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        var result = await Games.FindAsync(x => x.Id == gameId, cancellationToken: cancellationToken);

        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        await Games.ReplaceOneAsync(x => x.Id == game.Id, game, cancellationToken: cancellationToken);
    }

    public async Task InsertGameAsync(Game game, CancellationToken cancellationToken = default)
    {
        await Games.InsertOneAsync(game, cancellationToken: cancellationToken);
    }

    public async Task UpdateMiniGameAsync(MiniGameInstance miniGame, CancellationToken cancellationToken = default)
    {
        await MiniGameInstances
            .ReplaceOneAsync(
                x => x.Id == miniGame.Id,
                miniGame,
                cancellationToken: cancellationToken);
    }

    public async Task<MiniGameInstance> FindMiniGameAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await MiniGameInstances.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<GameDefinition> FindGameDefinitionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await GameDefinitions.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<MiniGameDefinition> FindMiniGameDefinitionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await MiniGameDefinitions.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
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

        var miniGames = MiniGameInstances.Find(x => x.GameId == gameId).ToList();

        var scores = new Dictionary<Guid, Dictionary<Guid, int>>();
        foreach (var miniGame in miniGames)
        {
            foreach (var score in miniGame.PlayerScores)
            {
                if (scores.ContainsKey(score.PlayerId))
                {
                    if (scores[score.PlayerId].ContainsKey(miniGame.Id))
                    {
                        scores[score.PlayerId][miniGame.Id] += score.Score;
                    }
                    else
                    {
                        scores[score.PlayerId].Add(miniGame.Id, score.Score);
                    }
                }
                else
                {
                    scores.Add(score.PlayerId, new Dictionary<Guid, int> {
                        { miniGame.Id, score.Score }
                    });
                }
            }
        }

        return scores.Select(x => (players.First(p => p.Id == x.Key), x.Value));
    }

    public async Task InsertMiniGameAsync(MiniGameInstance miniGame, CancellationToken cancellationToken = default)
    {
        await MiniGameInstances
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
        await MiniGameDefinitions
            .InsertManyAsync(gameDefinitions, cancellationToken: cancellationToken);
    }

    public async Task<(TState?, TDefinition?)> FindCurrentMiniGameStateAndDefinitionAsync<TState, TDefinition>(Guid gameId, CancellationToken cancellationToken = default)
        where TState : MiniGameStateData, new()
        where TDefinition : MiniGameDefinitionData, new()
    {
        var game = await FindGameAsync(gameId, cancellationToken);
        var miniGame = await FindMiniGameAsync(game.CurrentMiniGameId!.Value, cancellationToken);
        var miniGameDefinition = await FindMiniGameDefinitionAsync(miniGame.MiniGameDefinitionId, cancellationToken);
        var definition = miniGameDefinition.Definition.As<TDefinition>();
        var state = miniGame.State.As<TState>();

        return (state, definition);
    }

    public async Task<Room?> FindRoomByCodeAsync(string roomCode, CancellationToken cancellationToken = default)
    {
        var result = await Rooms.FindAsync(x => x.Code == roomCode, cancellationToken: cancellationToken);
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateRoomAsync(Room room, CancellationToken cancellationToken = default)
    {
        await Rooms.FindOneAndReplaceAsync(x => x.Code == room.Code, room, cancellationToken: cancellationToken);
    }

    public async Task InsertRoomAsync(Room room, CancellationToken cancellationToken = default)
    {
        await Rooms.InsertOneAsync(room, cancellationToken: cancellationToken);
    }

    public async Task UpdateMiniGameStatusAsync(Guid Id, string Status, CancellationToken cancellationToken = default)
    {
        await MiniGameInstances.UpdateOneAsync(x => x.Id == Id, Builders<MiniGameInstance>.Update.Set(x => x.MiniGameStatus, Status), cancellationToken: cancellationToken);
    }
}