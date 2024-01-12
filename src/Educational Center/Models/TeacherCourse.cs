using System.ComponentModel.DataAnnotations;

namespace Educational_Center.Models
{
    public class TeacherCourse
    {
        [Key]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
