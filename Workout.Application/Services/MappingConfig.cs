using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Application.Common.Dto;
using Workout.Domain.Entities;

namespace Workout.Application.Services
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<WorkoutPlan, WorkoutPlanDto>().ReverseMap();
                config.CreateMap<WorkoutExercise, WorkoutExerciseDto>()
                    .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.Name : null))
                    .ReverseMap();
                config.CreateMap<WorkoutComments, WorkoutCommentsDto>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                    .ReverseMap(); 
                config.CreateMap<ScheduleWorkout, ScheduleWorkoutDto>()
                    .ForMember(dest => dest.WorkoutName, opt => opt.MapFrom(src => src.Workout != null ? src.Workout.Name : null))
                    .ReverseMap();


            });
            return mappingConfig;
        }
    }
}

