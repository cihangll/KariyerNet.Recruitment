using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerNet.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class Created_Entity_DisabledWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppJobAdvertBenefits");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppJobAdvertBenefits");

            migrationBuilder.CreateTable(
                name: "AppDisabledWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDisabledWords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDisabledWords_Name_TenantId",
                table: "AppDisabledWords",
                columns: new[] { "Name", "TenantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDisabledWords");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppJobAdvertBenefits",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppJobAdvertBenefits",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
