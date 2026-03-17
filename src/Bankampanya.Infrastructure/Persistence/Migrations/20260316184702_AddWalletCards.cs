using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bankampanya.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wallet_cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BankName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CardType = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CustomName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_cards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_cards_UserId_BankName",
                table: "wallet_cards",
                columns: new[] { "UserId", "BankName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wallet_cards");
        }
    }
}
