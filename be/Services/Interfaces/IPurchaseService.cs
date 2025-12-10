using App.DTOs;

namespace App.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseDTO>> GetAllPurchasesAsync();
        Task<IEnumerable<PurchaseItemDTO>> GetPurchaseItemByPurchaseIdAsync(int purchaseId);
        Task<PurchaseDTO> GetPurchaseByIdAsync(int purchaseId);
        Task<PurchaseDTO> CreatePurchaseAsync(CreatePurchaseDTO dto);
        Task<PurchaseDTO> UpdatePurchaseAsync(int id, PurchaseDTO dto);
        Task<bool> DeletePurchaseAsync(int id);
        Task<PurchaseDTO> UpdatePurchaseStatusAsync(int id, UpdatePurchaseStatusDTO dto);

        Task<IEnumerable<PurchaseDTO>> GetPurchasesByUserIdAsync(string userId);
    }
}