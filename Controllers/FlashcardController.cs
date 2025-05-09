﻿using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using SquirrelCannon.Data;
using SquirrelCannon.Models;

namespace SquirrelCannon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardController : ControllerBase
    {
        private readonly FlashcardContext _context;

        public FlashcardController(FlashcardContext context)
        {
            _context = context;
        }

        [HttpGet("review/{subjectId}")]
        public async Task<IActionResult> GetCardstoReview(int subjectId)
        {
            var today = DateTime.Today;
            var cards = await _context.Flashcards
                .Where(c => c.SubjectId == subjectId)
                .ToListAsync();

            var cardsToReview = cards
                .Where(c => (today - c.LastReview).Days >=
                GetNextReviewInterval(c.Box)).ToList();

            return Ok(cardsToReview);
        }

        [HttpPost("review")]
        public async Task<IActionResult> Review([FromBody] ReviewModel model)
        {
            var card = await _context.Flashcards.FindAsync(model.Id);
            if (card == null) return NotFound();

            // Update flashcard state
            if (model.Correct && card.Box < 5)
                card.Box++;
            else if (!model.Correct)
                card.Box = 1;

            card.LastReview = DateTime.Today;

            // Insert into FlashcardReview
            var review = new FlashcardReview
            {
                FlashcardId = model.Id,
                ReviewedAt = DateTime.UtcNow, // Use UTC for consistency
                WasCorrect = model.Correct
                // UserId can be added later
            };
            _context.FlashcardReviews.Add(review);

            await _context.SaveChangesAsync();

            return Ok();
        }
        
        [HttpGet("today/correct-count")]
        public async Task<ActionResult<int>> GetTodaysCorrectCount()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            int count = await _context.FlashcardReviews
                .Where(r => r.WasCorrect && r.ReviewedAt >= today && r.ReviewedAt < tomorrow)
                .CountAsync();

            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FlashcardModel model)
        {
            if (ModelState.IsValid)
            {
                var flashcard = new Flashcard
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    SubjectId = model.SubjectId,
                    Box = 1,
                    LastReview = new DateTime(2024, 1, 1)
                };

                _context.Flashcards.Add(flashcard);
                await _context.SaveChangesAsync();

                return Ok(flashcard);

            }

            return BadRequest(ModelState);
        }

        [HttpGet("box-stats")]
        public IActionResult GetBoxStats() {

            var stats = _context.Flashcards
                .GroupBy(f => f.Box)
                .Select(g => new BoxStat { Box = g.Key, Count = g.Count() })
                .OrderBy(x => x.Box)
                .ToList();

            return Ok(stats);
        }

        [HttpGet("box-stats/{subjectId}")]
        public IActionResult GetBoxStats(int subjectId)
        {

            var stats = _context.Flashcards
                .Where(c => c.SubjectId == subjectId)
                .GroupBy(f => f.Box)
                .Select(g => new { Box = g.Key, Count = g.Count() })
                .OrderBy(x => x.Box)
                .ToList();

            return Ok(stats);
        }


        private int GetNextReviewInterval(int box) => box switch
        {
            1 => 1,
            2 => 2,
            3 => 4,
            4 => 8,
            5 => 16,
            6 => 32,
            7 => 64,
            8 => 36500,
            _ => 1
        };

    }

        public class ReviewModel
        { 
            public int Id { get; set; }
            public bool Correct { get; set; }
        }

        public class FlashcardModel
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public int SubjectId { get; set; }

        }
     }

