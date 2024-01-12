using Educational_Center.Models;

namespace Educational_Center.Data.ViewModels
{
    public class TeacherUpdateViewModel
    {
        public Teacher Teacher { get; set; }
        public List<Course> Courses { get; set; }
        public List<int> SelectedCourseIds { get; set; }
    }
}
