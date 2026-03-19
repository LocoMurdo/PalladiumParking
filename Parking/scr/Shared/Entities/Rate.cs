using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class Rate : BaseEntity
    {
        public VehicleType VehicleType { get; set; } = VehicleType.Car; // Car o Moto
        [Precision(16, 2)]
        public decimal PricePerHour { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public enum VehicleType
    {
        Car = 1,
        Motorcycle = 2
    }
}
