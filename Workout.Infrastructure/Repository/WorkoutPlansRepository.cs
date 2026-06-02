
using Workout.Domain.Entities;
using Workout.Infrastructure.Data;
using Workout.Application.Common.Interfaces;
using Workout.Application.Common.Dto;
using Microsoft.EntityFrameworkCore;
namespace Workout.Infrastructure.Repository
{
    public class WorkoutPlansRepository : Repository<WorkoutPlan>, IWorkoutPlansRepository
    {
        private readonly AppDbContext _db;
        public WorkoutPlansRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<WorkoutReportDto> GenerateReport(Guid userId)
        {
            var schedules = await _db.scheduleWorkouts
                .Include(s => s.Workout) 
                    .ThenInclude(w => w.WorkoutExercises)
                .Where(s => s.Workout.UserId == userId && s.IsCompleted == true)
                .ToListAsync();

            if (!schedules.Any())
            {
                return new WorkoutReportDto();
            }

            int totalWorkouts = schedules.Count;

            double totalVolume = schedules
                .SelectMany(s => s.Workout.WorkoutExercises)
                .Sum(we => we.Sets * we.Repetitions * we.Weight);

            var recentActivity = schedules
                .OrderByDescending(s => s.ScheduledDate)
                .Select(s => new RecentActivityDto
                {
                    Id = s.Id.ToString(),
                    Date = s.ScheduledDate,
                    WorkoutName = s.Workout.Name,
                    ExercisesCount = s.Workout.WorkoutExercises?.Count ?? 0
                })
                .ToList();

            var weeklyWorkouts = schedules
                .GroupBy(s => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    s.ScheduledDate,
                    System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday))
                .Select(g => new WeeklyWorkoutDto
                {
                    Week = $"W{g.Key}",
                    Count = g.Count(),
                    Volume = g.Sum(s => s.Workout.WorkoutExercises.Sum(we => we.Sets * we.Repetitions * we.Weight))
                })
                .OrderBy(w => w.Week)
                .ToList();

            return new WorkoutReportDto
            {
                TotalWorkouts = totalWorkouts,
                TotalVolume = totalVolume,
                StreakDays = 4, 
                WeeklyWorkouts = weeklyWorkouts,
                RecentActivity = recentActivity
            };
        }

        public async Task<IEnumerable<WorkoutPlanResponseDto>> GetAllWorkoutsByUserId(Guid userId)
        {
            IEnumerable<WorkoutPlanResponseDto> data = await _db.workoutPlans
                     .AsNoTrackingWithIdentityResolution()
                     .Where(wp => wp.UserId == userId)
                     .Select(wp => new WorkoutPlanResponseDto
                     {
                         Id = wp.Id,
                         Name = wp.Name,
                         Description = wp.Description
                     })
                     .ToListAsync();
            return data;
        }
        
        public void Update(WorkoutPlan model)
        {
            _db.workoutPlans.Update(model);
        }

    }
}
