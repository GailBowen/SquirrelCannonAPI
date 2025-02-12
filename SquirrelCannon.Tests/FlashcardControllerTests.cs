using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SquirrelCannon.Controllers;
using SquirrelCannon.Data;
using SquirrelCannon.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquirrelCannon.Tests
{
    [TestClass]
    public sealed class FlashcardControllerTests
    {
        private FlashcardController _controller;
        private Mock<FlashcardContext> _mockContext;
        private Mock<DbSet<Flashcard>> _mockFlashcards;

        [TestInitialize]
        public void Setup()
        {
            // Sample data for testing
            var flashcards = new List<Flashcard>
            {
                new Flashcard { Id = 1, Question = "Q1", Answer = "A1", SubjectId = 1, Box = 1 },
                new Flashcard { Id = 2, Question = "Q2", Answer = "A2", SubjectId = 2, Box = 2 }
            };

            // Mock DbSet
            _mockFlashcards = CreateMockDbSet(flashcards);

            // Mock DbContext
            _mockContext = new Mock<FlashcardContext>();
            _mockContext.Setup(c => c.Flashcards).Returns(_mockFlashcards.Object);

            // Initialize controller with mocked context
            _controller = new FlashcardController(_mockContext.Object);
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

            // Verify that Add and SaveChangesAsync were called
            _mockFlashcards.Verify(m => m.Add(It.IsAny<Flashcard>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        private static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.ToList().Add);

            return mockSet;
        }
    }
}
