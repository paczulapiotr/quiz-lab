using MongoDB.Bson.Serialization;
using Quiz.Master.Core.Models;

public class RoomClassMap : BsonClassMap<Room>
{
    public RoomClassMap()
    {
        AutoMap();
        MapIdMember(c => c.Id);
    }
}