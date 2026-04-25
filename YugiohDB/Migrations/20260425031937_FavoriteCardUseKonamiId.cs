using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YugiohDB.Migrations
{
    /// <inheritdoc />
    public partial class FavoriteCardUseKonamiId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteCards_Cards_CardId",
                table: "FavoriteCards");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "FavoriteCards",
                newName: "KonamiCardId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteCards_CardId_UserId",
                table: "FavoriteCards",
                newName: "IX_FavoriteCards_KonamiCardId_UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KonamiCardId",
                table: "FavoriteCards",
                newName: "CardId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteCards_KonamiCardId_UserId",
                table: "FavoriteCards",
                newName: "IX_FavoriteCards_CardId_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteCards_Cards_CardId",
                table: "FavoriteCards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
