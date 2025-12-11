using App.Data;

namespace App.Domain.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseDescription { get; set; } = null!;
        public string? CourseThumbnail { get; set; }
        public decimal CoursePrice { get; set; }
        public bool IsPublished { get; set; } = true;
        public int Discount { get; set; }

        // Quan hệ
        public string EducatorId { get; set; } = null!;
        public ApplicationUser Educator { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Chapter> CourseContent { get; set; } = new List<Chapter>();
        public ICollection<CourseRating> CourseRatings { get; set; } = new List<CourseRating>();
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
        public ICollection<Review> Reviews {get; set;} = new List<Review>();
    }
}