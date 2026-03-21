using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class SubscriptionPrice : BaseEntity
    {
        public VehicleType VehicleType { get; set; }
        public SubscriptionPlan Plan { get; set; }

        [Precision(16, 2)]
        public decimal Price { get; set; }
    }
}
