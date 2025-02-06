using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Entities;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamAPIController : ControllerBase
    {
        private readonly UniversityContext _context;

        public ExamAPIController(UniversityContext context)
        {
            _context = context;
        }

        // GET: api/ExamAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamEntity>>> GetExams()
        {
            return await _context.Exams.ToListAsync();
        }

        // GET: api/ExamAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamEntity>> GetExamEntity(int id)
        {
            var examEntity = await _context.Exams.FindAsync(id);

            if (examEntity == null)
            {
                return NotFound();
            }

            return examEntity;
        }

        // PUT: api/ExamAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamEntity(int id, ExamEntity examEntity)
        {
            if (id != examEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(examEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExamAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExamEntity>> PostExamEntity(ExamEntity examEntity)
        {
            _context.Exams.Add(examEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamEntity", new { id = examEntity.Id }, examEntity);
        }

        // DELETE: api/ExamAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamEntity(int id)
        {
            var examEntity = await _context.Exams.FindAsync(id);
            if (examEntity == null)
            {
                return NotFound();
            }

            _context.Exams.Remove(examEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamEntityExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}
