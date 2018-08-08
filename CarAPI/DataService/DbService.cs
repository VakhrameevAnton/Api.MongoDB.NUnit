using System;
using System.Linq;
using CarAPI.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarAPI.DataService
{
    public class DbService: IDbService
    {
        public IMongoDatabase Db;
        public AppSettings AppSettings;
        public IMongoClient MongoClient;

        public DbService(IOptions<AppSettings> options)
        {
            AppSettings = options.Value;
            MongoClient = new MongoClient(AppSettings.MongoConnectionString);
            Db = MongoClient.GetDatabase("car");
        }

        public void InsertCar(Car car)
        {
            var collection = Db.GetCollection<Car>("cars");
            collection.InsertOne(car);
        }

        public void UpdateCar(Car car, bool needUpdateDescription)
        {
            var collection = Db.GetCollection<Car>("cars");

            if (needUpdateDescription)
                collection.UpdateOne(Builders<Car>.Filter.Eq("_id", car.Id),
                    Builders<Car>.Update.Set("Description", car.Description),
                    new UpdateOptions {IsUpsert = false});
            collection.UpdateOne(Builders<Car>.Filter.Eq("_id", car.Id),
                Builders<Car>.Update.Set("Name", car.Name),
                new UpdateOptions {IsUpsert = false});

        }

        public Car GetCar(int carId)
        {
            var collection = Db.GetCollection<Car>("cars");

            var list = collection.Find(Builders<Car>.Filter.Eq("_id", carId)).ToList();
            return list.FirstOrDefault();
        }

        public int DeleteCar(int id)
        {
            var collection = Db.GetCollection<Car>("cars");

            var res = collection.DeleteOne(Builders<Car>.Filter.Eq("_id", id));
            return (int)res.DeletedCount;
        }
    }
}