namespace TestArchitecture.Data.Vehicles;

public interface IVehicleRepository
{
    Task <VehicleDto> AddAsync(CreateVehicle)
}