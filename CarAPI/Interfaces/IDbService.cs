namespace CarAPI.Interfaces
{
    public interface IDbService
    {
        void InsertCar(Car car);
        void UpdateCar(Car car, bool needUpdateDescription);
        Car GetCar(int carId);
        int DeleteCar(int id);
    }
}