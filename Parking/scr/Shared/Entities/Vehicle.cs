using Parking.API.scr.Shared.Entities.Common;

namespace Parking.API.scr.Shared.Entities
{
    public class Vehicle : BaseEntity, IAuditableEntity
    {
        public string PlateId { get; set; }    
        public string? CarModel { get; set; }
        
        public int? PersonId { get; set; }
        public Person? person { get; set; }
        public DateTime? CreatedAt { get; set;  }
        public DateTime? UpdatedAt {  get; set; }
    }
}
