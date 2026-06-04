using Microsoft.EntityFrameworkCore;
using Workout.Domain.Entities;

namespace Workout.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Exercise> exercises { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<WorkoutPlan> workoutPlans { get; set; }
        public DbSet<WorkoutExercise> workoutExercises { get; set; }
        public DbSet<WorkoutComments> workoutComments { get; set; }
        public DbSet<ScheduleWorkout> scheduleWorkouts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutComments>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // WorkoutPlans: frequently queried by UserId
            modelBuilder.Entity<WorkoutPlan>()
                .HasIndex(w => w.UserId)
                .HasDatabaseName("IX_WorkoutPlans_UserId");

            // ScheduleWorkouts: queried by WorkoutId and filtered by date
            modelBuilder.Entity<ScheduleWorkout>()
                .HasIndex(s => s.WorkoutId)
                .HasDatabaseName("IX_ScheduleWorkouts_WorkoutId");

            modelBuilder.Entity<ScheduleWorkout>()
                .HasIndex(s => s.ScheduledDate)
                .HasDatabaseName("IX_ScheduleWorkouts_ScheduledDate");

            // WorkoutComments: queried by WorkoutId
            modelBuilder.Entity<WorkoutComments>()
                .HasIndex(c => c.WorkoutId)
                .HasDatabaseName("IX_WorkoutComments_WorkoutId");

            modelBuilder.Entity<Exercise>().HasData(
                new Exercise { Id = 1, Name = "Barbell Bench Press", Description = "A classic compound chest exercise targeting pectorals, anterior deltoids, and triceps.", Category = "Chest" },
                new Exercise { Id = 2, Name = "Incline Dumbbell Press", Description = "An upper chest focused exercise targeting the clavicular head of the pectoralis major.", Category = "Chest" },
                new Exercise { Id = 3, Name = "Cable Chest Fly", Description = "An isolation exercise for chest definition and stretching the pectoral muscles.", Category = "Chest" },
                new Exercise { Id = 4, Name = "Barbell Back Squat", Description = "The king of lower body movements targeting quadriceps, glutes, hamstrings, and core.", Category = "Legs" },
                new Exercise { Id = 5, Name = "Leg Press", Description = "A machine-based lower body exercise prioritizing quadriceps and glutes with low spinal loading.", Category = "Legs" },
                new Exercise { Id = 6, Name = "Leg Extensions", Description = "An isolation machine exercise for developing definition in the quadriceps.", Category = "Legs" },
                new Exercise { Id = 7, Name = "Conventional Deadlift", Description = "A fundamental compound movement targeting the entire posterior chain, lower back, and hamstrings.", Category = "Back" },
                new Exercise { Id = 8, Name = "Lat Pulldown", Description = "An upper body vertical pull targeting the latissimus dorsi and biceps.", Category = "Back" },
                new Exercise { Id = 9, Name = "Barbell Row", Description = "A horizontal pulling movement for upper back thickness, lats, and rear deltoids.", Category = "Back" },
                new Exercise { Id = 10, Name = "Overhead Press", Description = "A vertical pressing compound exercise for building strong shoulders, traps, and triceps.", Category = "Shoulders" },
                new Exercise { Id = 11, Name = "Dumbbell Lateral Raise", Description = "An isolation exercise targeting the lateral deltoids for shoulder width.", Category = "Shoulders" },
                new Exercise { Id = 12, Name = "Barbell Bicep Curl", Description = "A classic arm isolation movement targeting the biceps brachii.", Category = "Arms" },
                new Exercise { Id = 13, Name = "Tricep Rope Pushdown", Description = "An isolation movement focusing on the lateral and medial heads of the triceps.", Category = "Arms" },
                new Exercise { Id = 14, Name = "Incline Bench Skull Crushers", Description = "An overhead elbow extension exercise prioritizing the long head of the triceps.", Category = "Arms" },
                new Exercise { Id = 15, Name = "Hanging Leg Raise", Description = "An advanced abdominal movement targeting the lower rectus abdominis.", Category = "Core" },
                new Exercise { Id = 16, Name = "Plank", Description = "An isometric core exercise targeting the transverse abdominis and lower back.", Category = "Core" },
                new Exercise { Id = 17, Name = "Assault Bike Interval", Description = "High intensity cardiovascular interval training engaging both upper and lower body.", Category = "Cardio" },
                new Exercise { Id = 18, Name = "Treadmill Run", Description = "Cardiovascular conditioning focused on running or walking.", Category = "Cardio" }
            );
        }
    }
}
