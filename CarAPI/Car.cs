using MongoDB.Bson.Serialization.Attributes;

namespace CarAPI
{
    public class Car
    {
        public int Id { get; set; }
        [BsonIgnoreIfNull]
        public string Name { get; set; } //[required]
        [BsonIgnoreIfNull]
        public string Description { get; set; } //[optional]
    }
}