using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YugiohDB.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atk = table.Column<long>(type: "bigint", nullable: false),
                    Def = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CardImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetRarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetRarityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardImage_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CardPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardmarketPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgplayerPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EbayPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmazonPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolstuffincPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPrices_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CardSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetRarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetRarityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardSets_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardImage_CardId",
                table: "CardImage",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_CardId",
                table: "CardPrices",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardId",
                table: "Cards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardSets_CardId",
                table: "CardSets",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardImage");

            migrationBuilder.DropTable(
                name: "CardPrices");

            migrationBuilder.DropTable(
                name: "CardSets");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
