using App.Domain.Models;
using App.Utils.Exceptions;
using AutoMapper;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IMapper _mapper;

    public PurchaseService(IPurchaseRepository purchaseRepository, IMapper mapper)
    {
        _purchaseRepository = purchaseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseDTO>> GetAllPurchasesAsync()
    {
        var purchases = await _purchaseRepository.GetAllPurchasesAsync();
        return _mapper.Map<IEnumerable<PurchaseDTO>>(purchases);
    }

    public async Task<IEnumerable<PurchaseItemDTO>> GetPurchaseItemByPurchaseIdAsync(int purchaseId)
    {
        var purchaseItems = await _purchaseRepository.GetPurchaseItemByPurchaseIdAsync(purchaseId);
        return _mapper.Map<IEnumerable<PurchaseItemDTO>>(purchaseItems);
    }

    public async Task<PurchaseDTO> UpdatePurchaseAsync(int id, PurchaseDTO dto)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(id);
        if (purchase == null)
        {
            throw new AppException(ErrorCode.FileNotFound, $"Không tìm thấy đơn mua với ID = {id}");
        }
        _mapper.Map(dto, purchase);
        await _purchaseRepository.UpdatePurchaseAsync(purchase);
        return _mapper.Map<PurchaseDTO>(purchase);
    }

    public async Task<bool> DeletePurchaseAsync(int id)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(id);
        if (purchase == null)
        {
            throw new AppException(ErrorCode.FileNotFound, $"Không tìm thấy đơn mua với ID = {id}");
        }
        await _purchaseRepository.DeletePurchaseAsync(id);
        return true;
    }

    public async Task<IEnumerable<PurchaseDTO>> GetPurchasesByUserIdAsync(string userId)
    {
        var purchases = await _purchaseRepository.GetAllPurchaseByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<PurchaseDTO>>(purchases);
    }
}