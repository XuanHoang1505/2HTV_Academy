namespace App.Repositories.Interfaces
{
    using App.Domain.Models;

    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);

        // Lấy giỏ hàng theo CartId
        Task<Cart?> GetCartByIdAsync(int cartId);

        // Tạo giỏ hàng mới
        Task<Cart> AddCartAsync(string userId);

        // Thêm khóa học vào giỏ
        Task AddCartItemAsync(CartItem cartItem);

        // Lấy CartItem theo CartId và CourseId
        Task<CartItem?> GetCartItemAsync(int cartId, int courseId);

        // Kiểm tra khóa học có trong giỏ chưa
        Task<bool> IsCourseInCartAsync(string userId, int courseId);

        // Xóa khóa học khỏi giỏ
        void RemoveCartItem(CartItem item);

        // Xóa toàn bộ giỏ hàng
        Task ClearCartAsync(string userId);

        // Tính tổng tiền giỏ hàng
        Task<decimal> GetCartTotalAsync(int cartId);

        // Đếm số khóa học trong giỏ
        Task<int> GetCartItemCountAsync(string userId);

        // Lưu thay đổi
        Task SaveChangesAsync();
    }
}