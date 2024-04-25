namespace eStoreOnline.Domain.Entities;

public class BaseEntity
{
    public int CreatedBy { get; set; }
    public int ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}