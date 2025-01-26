using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquirrelCannon.Data;

namespace SquirrelCannon.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly FlashcardContext _context;

        public SubjectsController(FlashcardContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {

            try
            {
                var subjects = await _context.Subjects.ToListAsync();

                return View(subjects);
                
            }
            catch (Exception)
            {

                return View("Loading");
            }
        
        }

        public async Task<IActionResult> Details(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
                return NotFound();

            return View(subject);
        }

    }
}
