using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YugiohDB.Migrations
{
    public partial class CreateYgoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BanlistInfo",
                columns: table => new
                {
                    Banlist_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ban_TCG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ban_OCG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ban_GOAT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanlistInfo", x => x.Banlist_Id);
                });

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
                    Level = table.Column<int>(type: "int", nullable: false),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Archetype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scale = table.Column<int>(type: "int", nullable: false),
                    LinkVal = table.Column<int>(type: "int", nullable: false),
                    BanlistInfoBanlist_Id = table.Column<int>(type: "int", nullable: true),
                    CardId1 = table.Column<int>(type: "int", nullable: true),
                    DeckId = table.Column<int>(type: "int", nullable: true),
                    DeckId1 = table.Column<int>(type: "int", nullable: true),
                    DeckId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_BanlistInfo_BanlistInfoBanlist_Id",
                        column: x => x.BanlistInfoBanlist_Id,
                        principalTable: "BanlistInfo",
                        principalColumn: "Banlist_Id");
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
                name: "CardSets",
                columns: table => new
                {
                    CardSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RarityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSets", x => x.CardSetId);
                    table.ForeignKey(
                        name: "FK_CardSets_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
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
                    ImageLocalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLocalUrlSmall = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "MiscInformation",
                columns: table => new
                {
                    MiscId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<long>(type: "bigint", nullable: false),
                    ViewsWeek = table.Column<int>(type: "int", nullable: false),
                    UpVotes = table.Column<int>(type: "int", nullable: false),
                    DownVotes = table.Column<int>(type: "int", nullable: false),
                    TreatedAs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OcgDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KonamiId = table.Column<int>(type: "int", nullable: false),
                    HasEffect = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscInformation", x => x.MiscId);
                    table.ForeignKey(
                        name: "FK_MiscInformation_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardMarket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcgPlayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ebay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amazon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoolStuffInc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Prices_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BanlistInfoBanlist_Id",
                table: "Cards",
                column: "BanlistInfoBanlist_Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_MiscInformation_CardId",
                table: "MiscInformation",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_CardId",
                table: "Prices",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardSets");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "MiscInformation");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "SetInformation");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "BanlistInfo");

            migrationBuilder.DropTable(
                name: "Decks");
        }
    }
}
