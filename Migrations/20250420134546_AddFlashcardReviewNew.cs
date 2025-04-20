using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SquirrelCannon.Migrations
{
    /// <inheritdoc />
    public partial class AddFlashcardReviewNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashcardReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlashcardId = table.Column<int>(type: "int", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WasCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardReviews_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardReviews_FlashcardId",
                table: "FlashcardReviews",
                column: "FlashcardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashcardReviews");
        }
    }
}
