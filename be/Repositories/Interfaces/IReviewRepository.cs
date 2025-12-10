using App.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace App.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> CreateAsync(Review review);
        Task<Review> GetByIdAsync(int id);
        Task<Review> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetByCourseIdAsync(int courseId);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(string userId, int courseId);
        Task<int> GetTotalReviewsCountAsync(int courseId);
        Task<double> GetAverageRatingAsync(int courseId);
        Task<Dictionary<int, int>> GetRatingDistributionAsync(int courseId);
    }
}