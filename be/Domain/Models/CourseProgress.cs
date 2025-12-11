using App.Data;

namespace App.Domain.Models
{
 public class CourseProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int CourseId { get; set; }
        public bool Completed { get; set; }
        public string LectureCompleted { get; set; } = null!; 
        public ApplicationUser User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}