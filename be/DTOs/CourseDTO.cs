namespace App.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseDescription { get; set; } = null!;
        public string? CourseThumbnail { get; set; }
        public decimal CoursePrice { get; set; }
        public bool IsPublished { get; set; }
        public int Discount { get; set; }

        public string? EducatorId { get; set; }
        public string? EducatorName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public IFormFile? CourseThumbnailFile { get; set; }

    }
}