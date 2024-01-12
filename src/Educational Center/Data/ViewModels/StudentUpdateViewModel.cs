using Educational_Center.Models;

namespace Educational_Center.Data.ViewModels
{
    public class StudentUpdateViewModel
    {
        public Student Student { get; set; }
        public List<Course> Courses { get; set; }
    }
}
