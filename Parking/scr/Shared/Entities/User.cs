using Parking.API.scr.Shared.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking.API.scr.Shared.Entities
{
    public class User : BaseEntity, IAuditableEntity
    {
        [Column("username")]

        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.Collaborator;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }

    public enum UserRole
    {
        Admin = 1,
        Collaborator = 2
    }
}
