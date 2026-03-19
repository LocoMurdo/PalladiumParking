using Parking.API.scr.Shared.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Parking.API.scr.Shared.Entities
{
    public class Person : BaseEntity, IAuditableEntity
    {
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    }
