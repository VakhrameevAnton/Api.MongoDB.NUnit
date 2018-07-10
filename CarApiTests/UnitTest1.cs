using System;
using System.Collections.Generic;
using System.Threading;
using CarAPI;
using CarAPI.DataService;
using CarAPI.Interfaces;
using CarAPI.RequestHandlers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CarApiTests
{
    [TestFixture]
    public class CarsRequestHandlerTest
    {
        private const int MaxRandom = 1000000;
        private int _carIdExecutedSuccessful;
        private int _carIdNotExecutedSuccessful;
        
        private Mock<IDbService> _dbServiceMock;

        private CarsRequestHandler _carsRequestHandler;

        [SetUp]
        public void Init()
        {
            _carIdExecutedSuccessful = new Random().Next(MaxRandom);
            _carIdNotExecutedSuccessful = new Random().Next(MaxRandom);
            _dbServiceMock = new Mock<IDbService>();
            _dbServiceMock.Setup(x => x.DeleteCar(_carIdExecutedSuccessful)).Returns(1);
            _dbServiceMock.Setup(x => x.GetCar(It.IsAny<int>())).Verifiable();
            _dbServiceMock.Setup(x => x.InsertCar(It.IsAny<Car>())).Verifiable();
            _dbServiceMock.Setup(x => x.GetCar(_carIdExecutedSuccessful))
                .Returns(new Car {Id = _carIdExecutedSuccessful});
                                    
            _carsRequestHandler = new CarsRequestHandler(_dbServiceMock.Object);            
        }

        [Test]
        public void GetCar_Successfu()
        {
            var res = _carsRequestHandler.GetCar(_carIdExecutedSuccessful);
            Assert.AreEqual(_carIdExecutedSuccessful, res.Id);
        }
        
        [Test]
        public void DeleteCar_Success()
        {
            var res = _carsRequestHandler.DeleteCar(_carIdExecutedSuccessful);
            Assert.AreEqual(1, res);
        }
        
        [Test]
        public void DeleteCar_UnSuccess()
        {
            var res = _carsRequestHandler.DeleteCar(_carIdNotExecutedSuccessful);
            Assert.AreEqual(0, res);
        }
        
        [Test]
        public void InsertCar_UnSuccess()
        {
            _carsRequestHandler.InsertCar(new Car {Id = _carIdNotExecutedSuccessful, Description = "desc"});
            _dbServiceMock.Verify(x => x.InsertCar(It.IsAny<Car>()), Times.Never, "Добавился лишний");
        }

        [Test]
        public void InsertCar_Success()
        {
            _carsRequestHandler.InsertCar(new Car() {Id = _carIdExecutedSuccessful, Description = "desc", Name = "name"});
            _dbServiceMock.Verify(x => x.InsertCar(It.IsAny<Car>()), Times.Once, "Не добавился");
        }

    }
}