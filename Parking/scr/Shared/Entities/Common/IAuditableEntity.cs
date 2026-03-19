namespace Parking.API.scr.Shared.Entities.Common
{
    public interface IAuditableEntity
    {
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
