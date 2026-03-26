using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class Payment : BaseEntity
    {
        public int? ParkingSessionId { get; set; } // Ahora es nullable
        public ParkingSession ParkingSession { get; set; }

       public  PaymentMethod Method  { get; set; }
        [Precision(16,2)]
        public decimal Amount { get; set; } 
        public DateTime PaymentDate { get; set; }
        public int CashRegisterId { get; set; }
        public CashRegister CashRegister { get; set; }

    }
    public enum PaymentMethod
{
    Cash = 1,
    Card = 2,
    Sinpe = 3
}
}
