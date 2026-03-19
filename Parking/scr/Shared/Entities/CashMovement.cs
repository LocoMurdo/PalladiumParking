using Microsoft.EntityFrameworkCore;
using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class CashMovement : BaseEntity , IAuditableEntity
    {
        public int CashRegisterId { get; set; }
        public CashRegister CashRegister { get; set; }

        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        [Precision(16, 2)]
        public decimal Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public enum CashMovementType
        {
            Income = 1,
            Expense = 2
        }

      
    }
}
