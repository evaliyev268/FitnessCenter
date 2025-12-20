using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenter.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceToTrainerFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_ServiceId",
                table: "Trainers",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Services_ServiceId",
                table: "Trainers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Services_ServiceId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_ServiceId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Trainers");
        }
    }
}
