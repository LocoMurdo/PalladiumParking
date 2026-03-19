using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class Subscriptions : BaseEntity
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int RateId { get; set; }
        public Rate Rate { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Active";
    }
}
