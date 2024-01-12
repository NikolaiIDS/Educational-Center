using Educational_Center.Models;
using Microsoft.EntityFrameworkCore;

namespace Educational_Center.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeacherCourse>()
              .HasKey(tc => new { tc.TeacherId, tc.CourseId });

            modelBuilder.Entity<TeacherCourse>()
                .HasOne(ma => ma.Teacher)
                .WithMany(m => m.TeacherCourses)
                .HasForeignKey(ma => ma.TeacherId);

            modelBuilder.Entity<TeacherCourse>()
                .HasOne(ma => ma.Course)
                .WithMany(a => a.TeacherCourses)
                .HasForeignKey(ma => ma.CourseId);
        }
    }

}
