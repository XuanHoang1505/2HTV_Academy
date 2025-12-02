using App.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        // DbSet các entity
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<CourseProgress> CourseProgresses { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Các cấu hình Identity (xóa tiền tố AspNet)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // --- Quan hệ Course - Category (1-n)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Quan hệ Course - Educator (1-n)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Educator)
                .WithMany(u => u.EducatorCourses)
                .HasForeignKey(c => c.EducatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Quan hệ Course - EnrolledStudents (n-n)
            modelBuilder.Entity<Course>()
                .HasMany(c => c.EnrolledStudents)
                .WithMany(u => u.EnrolledCourses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseEnrollment",
                    j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.Cascade),
                    j => { j.HasKey("CourseId", "UserId"); j.ToTable("CourseEnrollments"); });

            // --- Quan hệ Chapter - Course (1-n)
            modelBuilder.Entity<Chapter>()
                .HasOne(ch => ch.Course)
                .WithMany(c => c.CourseContent)
                .HasForeignKey(ch => ch.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Quan hệ Lecture - Chapter (1-n)
            modelBuilder.Entity<Lecture>()
                .HasOne(l => l.Chapter)
                .WithMany(ch => ch.ChapterContent)
                .HasForeignKey(l => l.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Quan hệ CourseRating - Course/User
            modelBuilder.Entity<CourseRating>()
                .HasOne(cr => cr.Course)
                .WithMany(c => c.CourseRatings)
                .HasForeignKey(cr => cr.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseRating>()
                .HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Quan hệ Purchase - User
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Quan hệ Purchase - PurchaseItem (1-n)
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Purchase)
                .WithMany(p => p.PurchaseItems)
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Quan hệ Course-PurchaseItem (1-n)
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Course)
                .WithMany(p => p.PurchaseItems)
                .HasForeignKey(pi => pi.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Quan hệ CourseProgress - Course/User
            modelBuilder.Entity<CourseProgress>()
                .HasOne(cp => cp.Course)
                .WithMany()
                .HasForeignKey(cp => cp.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseProgress>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.CourseProgresses)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Cart – User (1–1)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Cart – CartItem (1–n)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- CartItem – Course (1–n)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Course)
                .WithMany()
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
