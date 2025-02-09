using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DietFitter_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPossibilityForUserToLikeRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLikedRecommendations",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RecommendationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLikedRecommendations_UserDietRecommendations_Recommenda~",
                        column: x => x.RecommendationId,
                        principalSchema: "identity",
                        principalTable: "UserDietRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedRecommendations_RecommendationId",
                schema: "identity",
                table: "UserLikedRecommendations",
                column: "RecommendationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikedRecommendations",
                schema: "identity");
        }
    }
}
