using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YugiohDB.Migrations
{
    /// <inheritdoc />
    public partial class CreateYgoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.DeckId);
                });

            migrationBuilder.CreateTable(
                name: "SetInformation",
                columns: table => new
                {
                    SetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOfCards = table.Column<int>(type: "int", nullable: false),
                    TcgDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetInformation", x => x.SetId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KonamiCardId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atk = table.Column<int>(type: "int", nullable: true),
                    Def = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: true),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Archetype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scale = table.Column<int>(type: "int", nullable: true),
                    LinkVal = table.Column<int>(type: "int", nullable: true),
                    CardId1 = table.Column<int>(type: "int", nullable: true),
                    DeckId = table.Column<int>(type: "int", nullable: true),
                    DeckId1 = table.Column<int>(type: "int", nullable: true),
                    DeckId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_CardId1",
                        column: x => x.CardId1,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId");
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckId1",
                        column: x => x.DeckId1,
                        principalTable: "Decks",
                        principalColumn: "DeckId");
                    table.ForeignKey(
                        name: "FK_Cards_Decks_DeckId2",
                        column: x => x.DeckId2,
                        principalTable: "Decks",
                        principalColumn: "DeckId");
                });

            migrationBuilder.CreateTable(
                name: "BanlistInfo",
                columns: table => new
                {
                    BanlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    BanTCG = table.Column<string>(name: "Ban_TCG", type: "nvarchar(max)", nullable: true),
                    BanOCG = table.Column<string>(name: "Ban_OCG", type: "nvarchar(max)", nullable: true),
                    BanGOAT = table.Column<string>(name: "Ban_GOAT", type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanlistInfo", x => x.BanlistId);
                    table.ForeignKey(
                        name: "FK_BanlistInfo_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardMiscInformation",
                columns: table => new
                {
                    MiscId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    BetaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<long>(type: "bigint", nullable: false),
                    ViewsWeek = table.Column<int>(type: "int", nullable: false),
                    UpVotes = table.Column<int>(type: "int", nullable: false),
                    DownVotes = table.Column<int>(type: "int", nullable: false),
                    TreatedAs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OcgDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasEffect = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardMiscInformation", x => x.MiscId);
                    table.ForeignKey(
                        name: "FK_CardMiscInformation_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardPrices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    CardMarket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgPlayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ebay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amazon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolStuffInc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrices", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_CardPrices_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardSets",
                columns: table => new
                {
                    CardSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RarityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSets", x => x.CardSetId);
                    table.ForeignKey(
                        name: "FK_CardSets_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardImageId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrlSmall = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrlCropped = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalUrlSmall = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalUrlCropped = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Images_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BanlistInfo_CardId",
                table: "BanlistInfo",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardMiscInformation_CardId",
                table: "CardMiscInformation",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_CardId",
                table: "CardPrices",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardId1",
                table: "Cards",
                column: "CardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckId",
                table: "Cards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckId1",
                table: "Cards",
                column: "DeckId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckId2",
                table: "Cards",
                column: "DeckId2");

            migrationBuilder.CreateIndex(
                name: "IX_CardSets_CardId",
                table: "CardSets",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CardId",
                table: "Images",
                column: "CardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanlistInfo");

            migrationBuilder.DropTable(
                name: "CardMiscInformation");

            migrationBuilder.DropTable(
                name: "CardPrices");

            migrationBuilder.DropTable(
                name: "CardSets");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "SetInformation");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Decks");
        }
    }
}
