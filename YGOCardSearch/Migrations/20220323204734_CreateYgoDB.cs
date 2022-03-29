using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGOCardSearch.Migrations
{
    public partial class CreateYgoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atk = table.Column<long>(type: "bigint", nullable: false),
                    Def = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelId = table.Column<int>(type: "int", nullable: true),
                    DeckModelId = table.Column<int>(type: "int", nullable: true),
                    DeckModelId1 = table.Column<int>(type: "int", nullable: true),
                    DeckModelId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_CardModelId",
                        column: x => x.CardModelId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckModelId",
                        column: x => x.DeckModelId,
                        principalTable: "Decks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckModelId1",
                        column: x => x.DeckModelId1,
                        principalTable: "Decks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckModelId2",
                        column: x => x.DeckModelId2,
                        principalTable: "Decks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrlSmall = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Cards_CardModelId",
                        column: x => x.CardModelId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardmarketPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgplayerPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EbayPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmazonPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolstuffincPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Cards_CardModelId",
                        column: x => x.CardModelId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetRarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetRarityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Cards_CardModelId",
                        column: x => x.CardModelId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardModelId",
                table: "Cards",
                column: "CardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckModelId",
                table: "Cards",
                column: "DeckModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckModelId1",
                table: "Cards",
                column: "DeckModelId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckModelId2",
                table: "Cards",
                column: "DeckModelId2");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CardModelId",
                table: "Images",
                column: "CardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CardModelId",
                table: "Prices",
                column: "CardModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_CardModelId",
                table: "Sets",
                column: "CardModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Decks");
        }
    }
}
