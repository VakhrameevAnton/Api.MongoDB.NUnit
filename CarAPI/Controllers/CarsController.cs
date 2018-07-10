using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using CarAPI.RequestHandlers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly CarsRequestHandler _carsRequestHandler;

        public CarsController(CarsRequestHandler carsRequestHandler)
        {
            _carsRequestHandler = carsRequestHandler;
        }

        [HttpGet("{id}")]
        public Car Get(int id)
        {
            return _carsRequestHandler.GetCar(id);
        }

        [HttpPost]
        public void Post(Car newCar)
        {
            _carsRequestHandler.InsertCar(newCar);
        }

        [HttpPut]
        public void Update(Car newCar)
        {
            var stream = ControllerContext.HttpContext.Request.Body;
            var postParams = JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(stream).ReadToEnd());
            _carsRequestHandler.UpdateCar(newCar, postParams);
        }

        [HttpPost("{id}")]
        public HttpStatusCode Delete(int id)
        {
            var response = _carsRequestHandler.DeleteCar(id);
            return response > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }
    }
}