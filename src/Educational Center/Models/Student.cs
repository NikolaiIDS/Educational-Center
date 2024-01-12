using System.ComponentModel.DataAnnotations;

namespace Educational_Center.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string ContactNumber { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
