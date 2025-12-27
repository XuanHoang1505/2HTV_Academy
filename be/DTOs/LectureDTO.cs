namespace App.DTOs
{
    public class LectureDTO
    {
        public int Id { get; set; }
        public string LectureTitle { get; set; } = null!;
        public double LectureDuration { get; set; }
        public string? LectureUrl { get; set; }
        public bool IsPreviewFree { get; set; }
        public int LectureOrder { get; set; }
        public bool? IsCompleted { get; set; }
    }
}