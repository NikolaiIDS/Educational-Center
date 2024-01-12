using Educational_Center.Models;

namespace Educational_Center.Data.ViewModels
{
    public class CourseDisplayView
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }

        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
