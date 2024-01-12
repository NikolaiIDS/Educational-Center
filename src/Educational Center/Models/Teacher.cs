    using System.ComponentModel.DataAnnotations;

    namespace Educational_Center.Models
    {
        public class Teacher
        {
            [Key]
            public int TeacherId { get; set; }
            public string Photo { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Email { get; set; }

            public List<TeacherCourse> TeacherCourses { get; set; }
        }
    }
