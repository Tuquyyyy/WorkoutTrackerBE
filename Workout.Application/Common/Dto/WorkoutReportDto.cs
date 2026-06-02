using System;
using System.Collections.Generic;

namespace Workout.Application.Common.Dto
{
    public class WorkoutReportDto
    {
        public int TotalWorkouts { get; set; }
        public double TotalVolume { get; set; }
        public int StreakDays { get; set; }
        public int WorkoutsThisWeek { get; set; }
        public List<WeeklyWorkoutDto> WeeklyWorkouts { get; set; } = new();
        public List<RecentActivityDto> RecentActivity { get; set; } = new();
    }

    public class WeeklyWorkoutDto
    {
        public string Week { get; set; }
        public int Count { get; set; }
        public double Volume { get; set; }
    }

    public class RecentActivityDto
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string WorkoutName { get; set; }
        public int ExercisesCount { get; set; }
    }
}