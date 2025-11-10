using App.Data;

namespace App.Domain.Models
{
     public class Lecture
    {
        public int Id { get; set; }
        public string LectureId { get; set; } = null!;
        public string LectureTitle { get; set; } = null!;
        public double LectureDuration { get; set; }
        public string? LectureUrl { get; set; }
        public bool IsPreviewFree { get; set; }
        public int LectureOrder { get; set; }

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; } = null!;
    }
}