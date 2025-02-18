using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquirrelCannon.Controllers;
using SquirrelCannon.Data;
using SquirrelCannon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelCannon.Tests
{
    [TestClass]
    public sealed class FlashcardControllerTests
    {
        private FlashcardController _controller;
        private FlashcardContext _context;

        [TestInitialize]
        public void Setup()
        {
            // Use a unique in-memory database name for each test to avoid conflicts
            var options = new DbContextOptionsBuilder<FlashcardContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database name
                .Options;

            // Initialize the context with the in-memory database
            _context = new FlashcardContext(options);

            // Seed the in-memory database with initial data
            _context.Flashcards.AddRange(new List<Flashcard>
            {
                new Flashcard { Id = 782, Question = "Q1", Answer = "A1", SubjectId = 1, Box = 1, LastReview = DateTime.Today.AddDays(-2) },
                new Flashcard { Id = 783, Question = "Q2", Answer = "A2", SubjectId = 1, Box = 2, LastReview = DateTime.Today.AddDays(-5) }
            });
            _context.SaveChanges();

            // Initialize controller with the context
            _controller = new FlashcardController(_context);
        }

        [TestMethod]
        public async Task Create_AddsNewFlashcard()
        {
            // Arrange
            var model = new FlashcardModel { Question = "Cool Story", Answer = "ambages ambagis f", SubjectId = 1 };

            // Act
            var result = await _controller.Create(model);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var createdCard = okResult.Value as Flashcard;
            Assert.IsNotNull(createdCard);
            Assert.AreEqual("Cool Story", createdCard.Question);
            Assert.AreEqual("ambages ambagis f", createdCard.Answer);
            Assert.AreEqual(1, createdCard.SubjectId);

            // Verify that the card was added to the database
            var cardInDb = await _context.Flashcards.FindAsync(createdCard.Id);
            Assert.IsNotNull(cardInDb);
        }

        [TestMethod]
        public async Task GetCardstoReview_ReturnsCorrectCards()
        {
            // Arrange
            var subjectId = 1;

            // Act
            var result = await _controller.GetCardstoReview(subjectId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var cardsToReview = okResult.Value as List<Flashcard>;
            Assert.IsNotNull(cardsToReview);
            Assert.AreEqual(2, cardsToReview.Count); // Only one card is due for review
            Assert.AreEqual(782, cardsToReview[0].Id); // Verify the correct card is returned
        }

        [TestMethod]
        public async Task Review_UpdatesCardCorrectly()
        {
            // Arrange
            var reviewModel = new ReviewModel { Id = 782, Correct = true };

            // Act
            var result = await _controller.Review(reviewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

            // Verify that the card was updated correctly
            var updatedCard = await _context.Flashcards.FindAsync(reviewModel.Id);
            Assert.IsNotNull(updatedCard);
            Assert.AreEqual(2, updatedCard.Box); // Box should increment because the answer was correct
        }

        [TestMethod]
        public void GetBoxStats_ReturnsCorrectStats()
        {
            // Act
            var result = _controller.GetBoxStats();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var stats = okResult.Value as List<BoxStat>;
            Assert.IsNotNull(stats);

            // Verify stats
            var statBox1 = stats.FirstOrDefault(s => s.Box == 1);
            var statBox2 = stats.FirstOrDefault(s => s.Box == 2);

            Assert.AreEqual(1, statBox1.Count); // One card in Box 1
            Assert.AreEqual(1, statBox2.Count); // One card in Box 2
        }
    }
}
