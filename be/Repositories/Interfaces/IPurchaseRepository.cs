using App.Domain.Models;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAllPurchasesAsync();
    Task<Purchase?> GetByIdAsync(int id);
    Task<IEnumerable<PurchaseItem>> GetPurchaseItemByPurchaseIdAsync(int purchaseId);
    Task UpdatePurchaseAsync(Purchase purchase);
    Task DeletePurchaseAsync(int id);
    Task<IEnumerable<Purchase>> GetAllPurchaseByUserIdAsync(string userId);
}