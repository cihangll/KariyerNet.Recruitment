using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerNet.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class Updated_UniqueKey_Entity_Positions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppPositions_Name",
                table: "AppPositions");

            migrationBuilder.CreateIndex(
                name: "IX_AppPositions_Name_TenantId",
                table: "AppPositions",
                columns: new[] { "Name", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppPositions_Name_TenantId",
                table: "AppPositions");

            migrationBuilder.CreateIndex(
                name: "IX_AppPositions_Name",
                table: "AppPositions",
                column: "Name",
                unique: true);
        }
    }
}
