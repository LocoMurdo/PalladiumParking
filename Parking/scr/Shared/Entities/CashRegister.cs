using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class CashRegister : BaseEntity
    {
       
        [Precision(16,2)]
        public decimal OpeningAmount { get; set; }
        [Precision(16, 2)]
        public decimal? ClosingAmount { get; set; }

        public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        public CashStatus Status { get; set; } = CashStatus.Open;

        public ICollection<Payment>? payments { get; set; }

            
    }
    public enum CashStatus
    {
        Open = 1,
        Closed = 2
    }
}
