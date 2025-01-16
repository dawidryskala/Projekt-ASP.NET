using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courses = _service.GetAllCourses();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = _service.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Instructors = _service.GetAllInstructors(); // Pobieranie instruktorów
            return View();
        }

        [HttpPost]
        public IActionResult Create(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                _service.AddCourse(course);
                return RedirectToAction("Index");
            }
            ViewBag.Instructors = _service.GetAllInstructors(); // Pobieranie instruktorów
            return View(course);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _service.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Instructors = _service.GetAllInstructors(); // Pobieranie instruktorów
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateCourse(course);
                return RedirectToAction("Index");
            }
            ViewBag.Instructors = _service.GetAllInstructors(); // Pobieranie instruktorów
            return View(course);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _service.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}
