public interface IPurchaseService
{
    Task<IEnumerable<PurchaseDTO>> GetAllPurchasesAsync();
    Task<IEnumerable<PurchaseItemDTO>> GetPurchaseItemByPurchaseIdAsync(int purchaseId);
    Task<PurchaseDTO> UpdatePurchaseAsync(int id, PurchaseDTO dto);
    Task<bool> DeletePurchaseAsync(int id);
    Task<IEnumerable<PurchaseDTO>> GetPurchasesByUserIdAsync(string userId);
}