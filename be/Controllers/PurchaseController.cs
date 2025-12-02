using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/purchases")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPurchases()
    {
        var purchases = await _purchaseService.GetAllPurchasesAsync();
        return Ok(purchases);
    }

    [HttpGet("{purchaseId}")]
    public async Task<IActionResult> GetPurchaseItems(int purchaseId)
    {
        var items = await _purchaseService.GetPurchaseItemByPurchaseIdAsync(purchaseId);
        return Ok(items);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchase(int id, PurchaseDTO dto)
    {
        var updatedPurchase = await _purchaseService.UpdatePurchaseAsync(id, dto);
        return Ok(updatedPurchase);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchase(int id)
    {
        var result = await _purchaseService.DeletePurchaseAsync(id);
        return Ok(result);
    }

    [HttpGet("purchaseUser")]
    public async Task<IActionResult> GetPurchasesByUserId()
    {
        var userId = User.FindFirst("userId")?.Value;
        var purchases = await _purchaseService.GetPurchasesByUserIdAsync(userId);
        return Ok(purchases);
    }
}