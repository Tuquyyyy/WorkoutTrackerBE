using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workout.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToWorkoutComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "workoutComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_workoutComments_UserId",
                table: "workoutComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_workoutComments_users_UserId",
                table: "workoutComments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workoutComments_users_UserId",
                table: "workoutComments");

            migrationBuilder.DropIndex(
                name: "IX_workoutComments_UserId",
                table: "workoutComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "workoutComments");
        }
    }
}
