using Educational_Center.Data;
using Educational_Center.Data.ViewModels;
using Educational_Center.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Educational_Center.Controllers
{
    public class CourseController : Controller
    {
        public ApplicationDbContext _db { get; set; }

        public CourseController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var courses = _db.Courses.Include(c => c.TeacherCourses).Include(c => c.Students)
             .Select(course => new CourseDisplayView
             {
                 CourseId = course.CourseId,
                 CourseName = course.CourseName,
                 Duration = course.Duration,
                 Description = course.Description,
                 Students = course.Students.ToList(),
                 Teachers = course.TeacherCourses.Select(tc => _db.Teachers.FirstOrDefault(t => t.TeacherId == tc.TeacherId)).ToList()
             }).ToList();


            return View(courses);

        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CourseUpdate(int id)
        {
            var courses = _db.Courses.Include(c => c.TeacherCourses).Include(c => c.Students)
                .Where(i => i.CourseId == id)
                .Select(course => new CourseDisplayView
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    Duration = course.Duration,
                    Description = course.Description,
                    Students = course.Students.ToList(),
                    Teachers = course.TeacherCourses.Select(tc => _db.Teachers.FirstOrDefault(t => t.TeacherId == tc.TeacherId)).ToList()
                }).FirstOrDefault();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CourseUpdate(CourseDisplayView updatedCourse, int courseId)
        {

            // Retrieve the existing course from the database
            var existingCourse = await _db.Courses
                .FindAsync(courseId);

            if (existingCourse != null)
                {
                    existingCourse.CourseName = updatedCourse.CourseName;
                    existingCourse.Duration = updatedCourse.Duration;
                    existingCourse.Description = updatedCourse.Description;
                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            

            // If ModelState is not valid or the course is not found, return to the view
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CourseDelete(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            var hasStudents = await _db.Students.Where(x => x.CourseId == course.CourseId).ToListAsync();
            if (course != null && hasStudents != null)
            {
                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRemove(CourseDisplayView course, int id)
        {
            var studentToDelete = await _db.Students.FindAsync(id);
            
            if (studentToDelete != null) 
            {
                _db.Students.Remove(studentToDelete);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(CourseUpdate), course.CourseId);
            }
            return BadRequest();
            
        }

        [HttpPost]
        public async Task<IActionResult> TeacherRemove(CourseDisplayView course, int id)
        {
            var teatherToRemove = await _db.TeacherCourses.Where(x => x.TeacherId == id).FirstOrDefaultAsync();

            
            if (teatherToRemove != null)
            {
                _db.TeacherCourses.Remove(teatherToRemove);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(CourseUpdate), course);
            }
            return NotFound();
        }
    }
}
