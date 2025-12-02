public class CourseDetailDTO
{
    public int Id { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string CourseDescription { get; set; } = null!;
    public string? CourseThumbnail { get; set; }
    public decimal CoursePrice { get; set; }
    public bool IsPublished { get; set; }
    public int Discount { get; set; }

    public string EducatorName { get; set; } = null!;
    public string CategoryName { get; set; } = null!;

    public List<ChapterDTO> CourseContent { get; set; } = new List<ChapterDTO>();
    public List<CourseRatingDTO> CourseRatings { get; set; } = new List<CourseRatingDTO>();
}
