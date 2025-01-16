using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorService _service;

        public InstructorController(IInstructorService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var instructors = _service.GetAllInstructors();
            return View(instructors);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var instructor = _service.GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(InstructorViewModel instructor)
        {
            if (ModelState.IsValid)
            {
                _service.AddInstructor(instructor);
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var instructor = _service.GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpPost]
        public IActionResult Edit(InstructorViewModel instructor)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateInstructor(instructor);
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var instructor = _service.GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteInstructor(id); // Usuwanie instruktora w serwisie
            return RedirectToAction("Index"); // Przekierowanie na stronę główną
        }
    }
}
