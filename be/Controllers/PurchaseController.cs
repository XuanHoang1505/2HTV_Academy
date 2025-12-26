using App.DTOs;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    [Authorize(Roles = "Admin")]
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

            return Ok(new
            {
                success = true,
                message = "Lấy danh sách đơn mua thành công",
                data = purchases
            });
        }

        [HttpGet("{purchaseId}")]
        public async Task<IActionResult> GetPurchaseItems(int purchaseId)
        {
            var items = await _purchaseService.GetPurchaseItemByPurchaseIdAsync(purchaseId);

            return Ok(new
            {
                success = true,
                message = "Lấy chi tiết đơn mua thành công",
                data = items
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchase(int id, PurchaseDTO dto)
        {
            var updatedPurchase = await _purchaseService.UpdatePurchaseAsync(id, dto);

            if (updatedPurchase == null)
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy đơn mua"
                });

            return Ok(new
            {
                success = true,
                message = "Cập nhật đơn mua thành công",
                data = updatedPurchase
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var result = await _purchaseService.DeletePurchaseAsync(id);

            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy đơn mua"
                });

            return Ok(new
            {
                success = true,
                message = "Xóa đơn mua thành công"
            });
        }

        [HttpGet("purchaseUser")]
        [Authorize] // endpoint này thường là user thường
        public async Task<IActionResult> GetPurchasesByUserId()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new
                {
                    success = false,
                    message = "Unauthorized"
                });

            var purchases = await _purchaseService.GetPurchasesByUserIdAsync(userId);

            return Ok(new
            {
                success = true,
                message = "Lấy danh sách đơn mua của người dùng thành công",
                data = purchases
            });
        }
    }
}
