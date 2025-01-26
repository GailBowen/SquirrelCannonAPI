using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelCannon.Data;

namespace SquirrelCannon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly FlashcardContext _context;

        public SubjectsController(FlashcardContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects() {

            try
            {
                var subjects = await _context.Subjects.ToListAsync();

                return Ok(subjects);
                
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching subjects.");
            }
        
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
                return NotFound();

            return Ok(subject);
        }

    }
}
