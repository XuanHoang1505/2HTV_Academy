using App.Domain.Models;
using App.DTOs;
using App.Repositories.Interfaces;
using App.Services.Interfaces;
using App.Utils.Exceptions;
using AutoMapper;

namespace App.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return new CartDTO
                {
                    CartItems = new List<CartItemDTO>(),
                    TotalPrice = 0,
                    ItemCount = 0
                };
            }

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task AddCourseToCartAsync(string userId, int courseId)
        {
            // Kiểm tra khóa học có tồn tại không
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new AppException(ErrorCode.CourseNotFound, "Khóa học không tồn tại");

            // Lấy hoặc tạo giỏ hàng
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _cartRepository.AddCartAsync(userId);
                await _cartRepository.SaveChangesAsync();
            }

            // Kiểm tra khóa học đã có trong giỏ chưa
            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, courseId);
            if (existingItem != null)
                throw new AppException(ErrorCode.CourseAlreadyInCart, "Khóa học đã có trong giỏ hàng");

            // Thêm khóa học vào giỏ
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                CourseId = courseId,
                Price = course.CoursePrice,
                AddedAt = DateTime.UtcNow
            };

            await _cartRepository.AddCartItemAsync(cartItem);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveCourseFromCartAsync(string userId, int courseId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new AppException(ErrorCode.CartNotFound, "Giỏ hàng không tồn tại");

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, courseId);
            if (cartItem == null)
                throw new AppException(ErrorCode.CartItemNotFound, "Không tìm thấy khóa học trong giỏ hàng");

            _cartRepository.RemoveCartItem(cartItem);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new AppException(ErrorCode.CartNotFound, "Giỏ hàng không tồn tại");

            if (!cart.CartItems.Any())
                throw new AppException(ErrorCode.EmptyCart, "Giỏ hàng đã trống");

            await _cartRepository.ClearCartAsync(userId);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task<CartSummaryDTO> GetCartSummaryAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return new CartSummaryDTO
                {
                    ItemCount = 0,
                    Total = 0
                };
            }

            return new CartSummaryDTO
            {
                ItemCount = cart.CartItems.Count,
                Total = cart.CartItems.Sum(ci => ci.Price)
            };
        }

        public async Task<bool> IsCourseInCartAsync(string userId, int courseId)
        {
            return await _cartRepository.IsCourseInCartAsync(userId, courseId);
        }

        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
                throw new AppException(ErrorCode.CartNotFound, "Giỏ hàng không tồn tại");

            return await _cartRepository.GetCartTotalAsync(cartId);
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            return await _cartRepository.GetCartItemCountAsync(userId);
        }
    }
}