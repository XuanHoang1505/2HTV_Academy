
namespace App.DTOs
{
    public class ChapterDTO
    {
        public int Id { get; set; }
        public string ChapterId { get; set; } = null!;
        public int ChapterOrder { get; set; }
        public string ChapterTitle { get; set; } = null!;
        public int CourseId { get; set; }
    }
}