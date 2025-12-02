using App.Data;
using App.Domain.Enums;

namespace App.Domain.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
    }
}
