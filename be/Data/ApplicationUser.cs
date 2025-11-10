using System.ComponentModel.DataAnnotations;
using App.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool IsLocked { get; set; } = false;

         public ICollection<Course> EnrolledCourses { get; set; } = new List<Course>();
        public ICollection<Course> EducatorCourses { get; set; } = new List<Course>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<CourseProgress> CourseProgresses { get; set; } = new List<CourseProgress>();

    }
}
