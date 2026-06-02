
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

            // Tính số buổi tập trong tuần này (bắt đầu từ Thứ Hai)
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = DateTime.Today.AddDays(-6);
            }
            int workoutsThisWeek = schedules.Count(s => s.ScheduledDate.Date >= startOfWeek && s.ScheduledDate.Date <= DateTime.Today);

            // Tính chuỗi ngày tập liên tục (StreakDays)
            var completedDates = schedules
                .Select(s => s.ScheduledDate.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            int streak = 0;
            if (completedDates.Any())
            {
                var today = DateTime.Today;
                var checkDate = completedDates.First();
                if (checkDate == today || checkDate == today.AddDays(-1))
                {
                    streak = 1;
                    for (int i = 1; i < completedDates.Count; i++)
                    {
                        if (completedDates[i - 1].AddDays(-1) == completedDates[i])
                        {
                            streak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return new WorkoutReportDto
            {
                TotalWorkouts = totalWorkouts,
                TotalVolume = totalVolume,
                StreakDays = streak,
                WorkoutsThisWeek = workoutsThisWeek,
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
