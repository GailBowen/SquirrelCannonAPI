using Microsoft.EntityFrameworkCore;
using SquirrelCannon.Models;
using System.Collections.Generic;

namespace SquirrelCannon.Data
{
    public class FlashcardContext : DbContext
    {
        public FlashcardContext() { }

        public FlashcardContext(DbContextOptions<FlashcardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flashcard> Flashcards { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.Subject)
                .WithMany()
                .HasForeignKey(f => f.SubjectId);


            modelBuilder.Entity<Flashcard>()
                .Property(f => f.LastReview)
                .HasDefaultValue(new DateTime(2025, 1, 1));

            modelBuilder.Entity<Flashcard>()
                .Property(f => f.Box)
                .HasDefaultValue(1);
        }
    }

}
