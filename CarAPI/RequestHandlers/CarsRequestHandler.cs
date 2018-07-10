using System;
using System.Collections.Generic;
using System.Linq;
using CarAPI.DataService;
using CarAPI.Interfaces;

namespace CarAPI.RequestHandlers
{
    public class CarsRequestHandler
    {
        public readonly IDbService DbService;

        public CarsRequestHandler(IDbService dbService)
        {
            DbService = dbService;
        }
        
        public void InsertCar(Car newCar)
        {
            if (String.IsNullOrEmpty(newCar.Name))
                return;
            
            DbService.InsertCar(newCar);
        }

        public void UpdateCar(Car newCar, Dictionary<string, object> postParams)
        {
            bool needUpdateDescription = postParams.Any(i => i.Key.ToLower().Contains("description"));
            DbService.UpdateCar(newCar, needUpdateDescription);
        }

        public int DeleteCar(int id)
        {
            return DbService.DeleteCar(id);
        }

        public Car GetCar(int id)
        {
            return DbService.GetCar(id);
        }
    }
}