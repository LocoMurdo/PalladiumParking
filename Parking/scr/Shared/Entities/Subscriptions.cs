using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class Subscriptions : BaseEntity, IAuditableEntity
    {
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int RateId { get; set; }
        public Rate Rate { get; set; }

        public SubscriptionPlan Plan { get; set; }

        [Precision(16, 2)]
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public enum SubscriptionPlan
    {
        Daily = 1,
        Biweekly = 2,
        Monthly = 3
    }

    public enum SubscriptionStatus
    {
        Active = 1,
        Expired = 2,
        Cancelled = 3
    }
}
