namespace SquirrelCannon.Models
{
    public class FlashcardReview
    {
        public int Id { get; set; }
        public int FlashcardId { get; set; }
        public Flashcard Flashcard { get; set; }
        public DateTime ReviewedAt { get; set; }
        public bool WasCorrect { get; set; }
    }
}