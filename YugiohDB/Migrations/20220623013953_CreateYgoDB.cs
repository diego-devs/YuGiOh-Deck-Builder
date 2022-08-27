using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YugiohDB.Migrations
{
    public partial class CreateYgoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    InternalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atk = table.Column<long>(type: "bigint", nullable: false),
                    Def = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelInternalID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.InternalID);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_CardModelInternalID",
                        column: x => x.CardModelInternalID,
                        principalTable: "Cards",
                        principalColumn: "InternalID");
                });

            migrationBuilder.CreateTable(
                name: "CardImage",
                columns: table => new
                {
                    InternalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrlSmall = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelInternalID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardImage", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_CardImage_Cards_CardModelInternalID",
                        column: x => x.CardModelInternalID,
                        principalTable: "Cards",
                        principalColumn: "InternalID");
                });

            migrationBuilder.CreateTable(
                name: "CardPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardMarket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgPlayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ebay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amazon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolStuffInc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelInternalID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPrices_Cards_CardModelInternalID",
                        column: x => x.CardModelInternalID,
                        principalTable: "Cards",
                        principalColumn: "InternalID");
                });

            migrationBuilder.CreateTable(
                name: "CardSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RarityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardModelInternalID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardSets_Cards_CardModelInternalID",
                        column: x => x.CardModelInternalID,
                        principalTable: "Cards",
                        principalColumn: "InternalID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardImage_CardModelInternalID",
                table: "CardImage",
                column: "CardModelInternalID");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_CardModelInternalID",
                table: "CardPrices",
                column: "CardModelInternalID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardModelInternalID",
                table: "Cards",
                column: "CardModelInternalID");

            migrationBuilder.CreateIndex(
                name: "IX_CardSets_CardModelInternalID",
                table: "CardSets",
                column: "CardModelInternalID");
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
