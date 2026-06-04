using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workout.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_workoutPlans_UserId",
                table: "workoutPlans",
                newName: "IX_WorkoutPlans_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_workoutComments_WorkoutId",
                table: "workoutComments",
                newName: "IX_WorkoutComments_WorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_scheduleWorkouts_WorkoutId",
                table: "scheduleWorkouts",
                newName: "IX_ScheduleWorkouts_WorkoutId");

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Chest", "A classic compound chest exercise targeting pectorals, anterior deltoids, and triceps.", "Barbell Bench Press" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Chest", "An upper chest focused exercise targeting the clavicular head of the pectoralis major.", "Incline Dumbbell Press" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Chest", "An isolation exercise for chest definition and stretching the pectoral muscles.", "Cable Chest Fly" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Legs", "The king of lower body movements targeting quadriceps, glutes, hamstrings, and core.", "Barbell Back Squat" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Legs", "A machine-based lower body exercise prioritizing quadriceps and glutes with low spinal loading.", "Leg Press" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Legs", "An isolation machine exercise for developing definition in the quadriceps.", "Leg Extensions" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Back", "A fundamental compound movement targeting the entire posterior chain, lower back, and hamstrings.", "Conventional Deadlift" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Back", "An upper body vertical pull targeting the latissimus dorsi and biceps.", "Lat Pulldown" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Back", "A horizontal pulling movement for upper back thickness, lats, and rear deltoids.", "Barbell Row" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Shoulders", "A vertical pressing compound exercise for building strong shoulders, traps, and triceps.", "Overhead Press" });

            migrationBuilder.InsertData(
                table: "exercises",
                columns: new[] { "Id", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { 11, "Shoulders", "An isolation exercise targeting the lateral deltoids for shoulder width.", "Dumbbell Lateral Raise" },
                    { 12, "Arms", "A classic arm isolation movement targeting the biceps brachii.", "Barbell Bicep Curl" },
                    { 13, "Arms", "An isolation movement focusing on the lateral and medial heads of the triceps.", "Tricep Rope Pushdown" },
                    { 14, "Arms", "An overhead elbow extension exercise prioritizing the long head of the triceps.", "Incline Bench Skull Crushers" },
                    { 15, "Core", "An advanced abdominal movement targeting the lower rectus abdominis.", "Hanging Leg Raise" },
                    { 16, "Core", "An isometric core exercise targeting the transverse abdominis and lower back.", "Plank" },
                    { 17, "Cardio", "High intensity cardiovascular interval training engaging both upper and lower body.", "Assault Bike Interval" },
                    { 18, "Cardio", "Cardiovascular conditioning focused on running or walking.", "Treadmill Run" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleWorkouts_ScheduledDate",
                table: "scheduleWorkouts",
                column: "ScheduledDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScheduleWorkouts_ScheduledDate",
                table: "scheduleWorkouts");

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutPlans_UserId",
                table: "workoutPlans",
                newName: "IX_workoutPlans_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutComments_WorkoutId",
                table: "workoutComments",
                newName: "IX_workoutComments_WorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleWorkouts_WorkoutId",
                table: "scheduleWorkouts",
                newName: "IX_scheduleWorkouts_WorkoutId");

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "An exercise that targets the chest, shoulders, and triceps.", "Push-up" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "A lower body exercise that targets the thighs and glutes.", "Squat" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "An upper body exercise that works the back and biceps.", "Pull-up" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Core", "A core exercise that targets the abdominals and lower back.", "Plank" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "A lower body exercise that works the legs and glutes.", "Lunge" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "An exercise that focuses on the biceps using weights.", "Bicep Curl" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "A strength exercise that targets the entire body, especially the back and legs.", "Deadlift" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Strength", "A chest exercise performed with a barbell or dumbbells.", "Bench Press" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Core", "An abdominal exercise that targets the upper abs.", "Crunch" });

            migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { "Cardio", "A full-body exercise that combines a squat, push-up, and jump.", "Burpee" });
        }
    }
}
