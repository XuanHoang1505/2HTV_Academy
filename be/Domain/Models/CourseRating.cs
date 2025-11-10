using App.Data;

namespace App.Domain.Models
{
    public class CourseRating
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int Rating { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}