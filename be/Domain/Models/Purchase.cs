using App.Data;
using App.Domain.Enums;

namespace App.Domain.Models
{
 public class Purchase
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public decimal Amount { get; set; }

        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}