using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerNet.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class Created_Entity_JobAdvertBenefits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppJobAdvertBenefits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobAdvertId = table.Column<Guid>(type: "uuid", nullable: false),
                    BenefitId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobAdvertBenefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppJobAdvertBenefits_AppBenefits_BenefitId",
                        column: x => x.BenefitId,
                        principalTable: "AppBenefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobAdvertBenefits_BenefitId",
                table: "AppJobAdvertBenefits",
                column: "BenefitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppJobAdvertBenefits");
        }
    }
}
