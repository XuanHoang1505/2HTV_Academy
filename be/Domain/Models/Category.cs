
using App.Data;

namespace App.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Educator { get; set; } = null!; 

        public ApplicationUser EducatorUser { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
