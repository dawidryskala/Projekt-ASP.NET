using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _service;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public EnrollmentController(
            IEnrollmentService service,
            IStudentService studentService,
            ICourseService courseService)
        {
            _service = service;
            _studentService = studentService;
            _courseService = courseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var enrollments = _service.GetAllEnrollments();
            return View(enrollments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Students = _studentService.FindAll();
            ViewBag.Courses = _courseService.GetAllCourses();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EnrollmentViewModel enrollment)
        {
            if (ModelState.IsValid)
            {
                _service.AddEnrollment(enrollment);
                return RedirectToAction("Index");
            }

            ViewBag.Students = _studentService.FindAll();
            ViewBag.Courses = _courseService.GetAllCourses();
            return View(enrollment);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var enrollment = _service.GetEnrollmentById(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            ViewBag.Students = _studentService.FindAll();
            ViewBag.Courses = _courseService.GetAllCourses();
            return View(enrollment);
        }

        [HttpPost]
        public IActionResult Edit(EnrollmentViewModel enrollment)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateEnrollment(enrollment);
                return RedirectToAction("Index");
            }

            ViewBag.Students = _studentService.FindAll();
            ViewBag.Courses = _courseService.GetAllCourses();
            return View(enrollment);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var enrollment = _service.GetEnrollmentById(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var enrollment = _service.GetEnrollmentById(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            return View(enrollment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteEnrollment(id);
            return RedirectToAction("Index");
        }
    }
}
