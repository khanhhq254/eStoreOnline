using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eStoreOnline.Domain.Entities;

public class BaseEntity
{
    public string CreatedBy { get; set; } = string.Empty;
    [ForeignKey(nameof(CreatedBy))]
    public IdentityUser CreatedByUser { get; set; } = default!;
    public string ModifiedBy { get; set; } = string.Empty;

    [ForeignKey(nameof(ModifiedBy))]
    public IdentityUser ModifiedByUser { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}