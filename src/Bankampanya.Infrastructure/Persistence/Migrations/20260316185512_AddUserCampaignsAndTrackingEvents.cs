using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bankampanya.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCampaignsAndTrackingEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProgressCurrent = table.Column<int>(type: "integer", nullable: false),
                    ProgressTarget = table.Column<int>(type: "integer", nullable: false),
                    JoinedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RewardedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_campaigns_campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_campaigns_tracking_templates_TrackingTemplateId",
                        column: x => x.TrackingTemplateId,
                        principalTable: "tracking_templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tracking_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserCampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MerchantName = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AmountText = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Qualified = table.Column<bool>(type: "boolean", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracking_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tracking_events_user_campaigns_UserCampaignId",
                        column: x => x.UserCampaignId,
                        principalTable: "user_campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tracking_events_UserCampaignId_OccurredAtUtc",
                table: "tracking_events",
                columns: new[] { "UserCampaignId", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_user_campaigns_CampaignId",
                table: "user_campaigns",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_user_campaigns_TrackingTemplateId",
                table: "user_campaigns",
                column: "TrackingTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_user_campaigns_UserId_CampaignId",
                table: "user_campaigns",
                columns: new[] { "UserId", "CampaignId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tracking_events");

            migrationBuilder.DropTable(
                name: "user_campaigns");
        }
    }
}
