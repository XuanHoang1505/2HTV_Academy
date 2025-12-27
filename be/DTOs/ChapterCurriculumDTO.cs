namespace App.DTOs
{
    public class ChapterCurriculumDTO
    {
        public int Id { get; set; }
        public string ChapterId { get; set; } = null!;
        public int ChapterOrder { get; set; }
        public string ChapterTitle { get; set; } = null!;
        public List<LectureDTO> Lectures { get; set; } = new List<LectureDTO>();
    }
}