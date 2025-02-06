using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ExamsController : Controller
    {
        // GET: ExamsController
        public ActionResult Index()
        {
            IEnumerable<ExamEntity>? exams = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7209/api/");
                //HTTP GET
                var responseTask = client.GetAsync("exam");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<List<ExamEntity>>();
                    readTask.Wait();
                    exams = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    exams = Enumerable.Empty<ExamEntity>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(exams);
        }
    }
}
