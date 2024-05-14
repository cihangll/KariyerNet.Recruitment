using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerNet.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_Relations_JobAdvertBenefits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobAdvertBenefits_AppJobAdverts_JobAdvertId",
                table: "AppJobAdvertBenefits");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobAdvertBenefits_AppJobAdverts_JobAdvertId",
                table: "AppJobAdvertBenefits",
                column: "JobAdvertId",
                principalTable: "AppJobAdverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobAdvertBenefits_AppJobAdverts_JobAdvertId",
                table: "AppJobAdvertBenefits");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobAdvertBenefits_AppJobAdverts_JobAdvertId",
                table: "AppJobAdvertBenefits",
                column: "JobAdvertId",
                principalTable: "AppJobAdverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
