<<<<<<< HEAD
public class ChapterDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
=======
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
>>>>>>> 062140932ade7e9de033c5336a5e22a9f9c364c5
}