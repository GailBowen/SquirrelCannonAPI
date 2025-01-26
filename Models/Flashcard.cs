namespace SquirrelCannon.Models
{
    public class Flashcard
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Box { get; set; } = 1;
        public DateTime LastReview { get; set; } = new DateTime(2025, 1, 2);
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }

}
