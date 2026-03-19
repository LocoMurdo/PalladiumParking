namespace Parking.API.scr.Shared.Interfaces.Persistence.Repositories
{
    public interface IVehicleRepository
    {
        Task<bool> VehicleExistAsync(string plateId);

    }
}
