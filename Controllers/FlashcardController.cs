// File: /Controllers/FlashcardController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SquirrelCannon.Data;
using SquirrelCannon.Models;
using SquirrelCannon.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelCannon.Controllers
{
    public class FlashcardController : Controller
    {
        private readonly FlashcardContext _context;

        public FlashcardController(FlashcardContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int subjectId)
        {
            var today = DateTime.Today;
            var cards = await _context.Flashcards
                .Where(c => c.SubjectId == subjectId)
                .ToListAsync();

            var cardsToReview = cards
                .Where(c => (today - c.LastReview).Days >=
                GetNextReviewInterval(c.Box)).ToList();

            ViewBag.SubjectId = subjectId;

            return View(cardsToReview);
        }

        [HttpPost]
        public async Task<IActionResult> Review(int id, bool correct)
        {
            var card = await _context.Flashcards.FindAsync(id);
            if (card == null) return NotFound();

            if (correct && card.Box < 5)
                card.Box++;
            else if (!correct)
                card.Box = 1;

            card.LastReview = DateTime.Today;
            await _context.SaveChangesAsync();

            return Ok();
        }

        public IActionResult Create()
        {
            var viewModel = new FlashcardViewModel
            {
                Subjects = _context.Subjects.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Title
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlashcardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var flashcard = new Flashcard
                {
                    Question = viewModel.Question,
                    Answer = viewModel.Answer,
                    SubjectId = viewModel.SubjectId,
                    Box = 1,
                    LastReview = new DateTime(2024, 1, 1)
                };

                _context.Flashcards.Add(flashcard);
                await _context.SaveChangesAsync();

                // Add success message
                TempData["SuccessMessage"] = "Flashcard added successfully!";

                // Clear the form
                ModelState.Clear();
                viewModel = new FlashcardViewModel();
            }

            // Repopulate the Subjects list
            viewModel.Subjects = _context.Subjects.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Title
            }).ToList();

            return View(viewModel);
        }


        private int GetNextReviewInterval(int box) => box switch
        {
            1 => 1,
            2 => 2,
            3 => 4,
            4 => 7,
            5 => 14,
            _ => 1
        };
    }
}
