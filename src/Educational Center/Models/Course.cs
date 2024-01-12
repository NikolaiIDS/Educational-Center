using System.ComponentModel.DataAnnotations;

namespace Educational_Center.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }

        public List<Student> Students { get; set; }
        public List<TeacherCourse> TeacherCourses { get; set; }
    }
}
