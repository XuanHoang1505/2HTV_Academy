namespace App.Repositories.Interfaces
{
    using App.Domain.Models;

    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);

        Task<Cart?> GetCartByIdAsync(int cartId);

        Task<Cart> AddCartAsync(string userId);

        Task AddCartItemAsync(CartItem cartItem);

        Task<CartItem?> GetCartItemAsync(int cartId, int courseId);

        Task<bool> IsCourseInCartAsync(string userId, int courseId);

        void RemoveCartItem(CartItem item);

        Task ClearCartAsync(string userId);

        Task<decimal> GetCartTotalAsync(int cartId);

        Task<int> GetCartItemCountAsync(string userId);
        Task SaveChangesAsync();
    }
}