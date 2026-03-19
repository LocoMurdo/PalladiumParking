using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;
using System;

namespace Parking.API.scr.Shared.Entities
{
    public class ParkingSession: BaseEntity ,IAuditableEntity
    {
   
        public string VisitorPlate { get;  set; } = default!;
        public int RateId { get;  set; }
        public Rate Rate { get; set; }
        public DateTime? EntryTime { get; set; }   
        public DateTime? ExitTime { get; set; }
        [Precision(16, 2)]
        public decimal? TotalAmount { get; set; }

        public ParkingSessionStatus Status { get; set; } = ParkingSessionStatus.Open;

        public ICollection<Payment>? Payments { get; set; } = new List<Payment>();
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public enum ParkingSessionStatus
        {
            Open = 1,
            Closed = 2,
            Cancelled = 3
        }
    }
}
