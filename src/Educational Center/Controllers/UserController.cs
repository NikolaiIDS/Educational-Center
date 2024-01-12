using Educational_Center.Data;
using Educational_Center.Data.ViewModels;
using Educational_Center.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;

namespace Educational_Center.Controllers
{
    public class UserController : Controller
    {
        public ApplicationDbContext _db { get; set; }

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> StudentView()
        {
            var students = _db.Students.ToList();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> StudentCreate()
        {
            var forView = new StudentUpdateViewModel { Courses = await _db.Courses.ToListAsync(), Student = new Student() };
            return View(forView);
        }

        [HttpPost]
        public async Task<IActionResult> StudentCreate(Student student)
        {
            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(StudentView));
        }

        [HttpGet]
        public async Task<IActionResult> StudentUpdate(int studentId)
        {
            var student = await _db.Students.FindAsync(studentId);
            var courses = await _db.Courses.ToListAsync();
            var forView = new StudentUpdateViewModel { Courses = courses, Student = student };
            return View(forView);
        }

        [HttpPost]
        public async Task<IActionResult> StudentUpdate(int studentId, Student student)
        {
            var fromDb = await _db.Students.FindAsync(studentId);
            fromDb.CourseId = student.CourseId;
            fromDb.DateOfBirth = student.DateOfBirth;
            fromDb.FirstName = student.FirstName;
            fromDb.LastName = student.LastName;
            fromDb.Age = student.Age;
            fromDb.ContactNumber = student.ContactNumber;
            fromDb.Photo = student.Photo;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(StudentView));
        }

        [HttpGet]
        public async Task<IActionResult> StudentDelete(int studentId)
        {
            var student = await _db.Students.FindAsync(studentId);

            if (student != null)
            {
                _db.Students.Remove(student);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(StudentView));
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> TeacherView()
        {
            var teachers = await _db.Teachers
                .Include(t => t.TeacherCourses)
                .ThenInclude(tc => tc.Course)
                .ToListAsync();

            // Create a list to hold the view models for each teacher
            var forView = teachers.Select(teacher => new TeacherUpdateViewModel
            {
                Teacher = teacher,
                Courses = teacher.TeacherCourses?.Select(tc => tc.Course).ToList() ?? new List<Course>()
            }).ToList();

            return View(forView);
        }

        [HttpGet]
        public async Task<IActionResult> TeacherCreate()
        {
            var forView = new TeacherUpdateViewModel { Teacher = new Teacher(), Courses = await _db.Courses.ToListAsync() };
            return View(forView);
        }

        [HttpPost]
        public async Task<IActionResult> TeacherCreate(Teacher teacher, List<Course> courses)
        {
            foreach (var item in courses)
            {
                TeacherCourse forDb = new TeacherCourse { TeacherId = teacher.TeacherId, CourseId = item.CourseId };
                await _db.TeacherCourses.AddAsync(forDb);
            }
            await _db.Teachers.AddAsync(teacher);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(TeacherView));
        }

        [HttpGet]
        public async Task<IActionResult> TeacherUpdate(int teacherId)
        {

            var teacher = await _db.Teachers.FindAsync(teacherId);
            var courses = await _db.Courses.ToListAsync();
            TeacherUpdateViewModel forView = new TeacherUpdateViewModel
            {
                Courses = courses,
                Teacher = teacher
            };
            return View(forView);
        }

        [HttpPost]
        public async Task<IActionResult> TeacherUpdate(int teacherId, TeacherUpdateViewModel fromView)
        {
            List<Course> courses = new List<Course>();
            var fromDb = await _db.Teachers.FindAsync(teacherId);
            if (fromView.SelectedCourseIds != null)
            {
                foreach (var item in fromView.SelectedCourseIds)
                {
                    var course = await _db.Courses.FindAsync(item);
                    if (course != null)
                    {
                        courses.Add(course);
                    }
                    
                }
                foreach (var course in courses)
                {
                    TeacherCourse forDb = new TeacherCourse { TeacherId = teacherId, CourseId = course.CourseId };
                    await _db.TeacherCourses.AddAsync(forDb);
                }
            }
            
            fromDb.TeacherId = teacherId;
            fromDb.DateOfBirth = fromView.Teacher.DateOfBirth;
            fromDb.FirstName = fromView.Teacher.FirstName;
            fromDb.LastName = fromView.Teacher.LastName;
            fromDb.Email = fromView.Teacher.Email;
            fromDb.Photo = fromView.Teacher.Photo;
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(TeacherView));
        }

        [HttpGet]
        public async Task<IActionResult> TeacherDelete(int teacherId)
        {
            var teacher = await _db.Teachers.FindAsync(teacherId);

            if (teacher != null)
            {
                _db.Teachers.Remove(teacher);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(TeacherView));
            }
            return NotFound();
        }
    }
}
