using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerNet.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Entity_Positions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppPositions",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppPositions");
        }
    }
}
