namespace eStoreOnline.Domain.Entities;

public class Cart : BaseEntity
{
    public int Id { get; set; }
    public List<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    public decimal TotalPrice { get; set; }

    public int UserId { get; set; }
    // public User User { get; set; }
}