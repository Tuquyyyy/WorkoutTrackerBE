using AutoMapper;
using System.Security.Claims;
using Workout.Application.Common.Dto;
using Workout.Application.Common.Interfaces;
using Workout.Application.Common.Result;
using Workout.Application.Errors;
using Workout.Application.Services.Interface;
using Workout.Domain.Entities;

namespace Workout.Application.Services.Implementation
{
    public class ScheduleWorkoutService : IScheduleWorkoutService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IWorkoutPlanService _workoutPlanService;

        public ScheduleWorkoutService(IUnitOfWork unitOfWork, IMapper mapper,IAuthService authService, IWorkoutPlanService workoutPlanService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
            _workoutPlanService = workoutPlanService;
        }

        public async Task<Result<string>> DeleteScheduledWorkout(Guid scheduleWorkoutId, ClaimsPrincipal user)
        {
            Result<Guid> getUserResult = _authService.GetUserId(user);
            if (getUserResult.IsFailure)
            {
                return Result<string>.Failure(getUserResult.Error);
            }
            Result<ScheduleWorkout> accessResult = await CheckAccess(scheduleWorkoutId, getUserResult.Values);
            if (accessResult.IsFailure) 
            {
                return Result<string>.Failure(accessResult.Error);
            }
            _unitOfWork.scheduleWorkouts.Remove(accessResult.Values);
            await _unitOfWork.Save();

            return Result<string>.Success("Scheduled workout deleted successfully.");

        }

        public async Task<Result<IEnumerable<ScheduleWorkoutDto>>> GetScheduleWorkoutsByUserId(ClaimsPrincipal user)
        {
            Result<Guid> getUserResult = _authService.GetUserId(user);
            if (getUserResult.IsFailure)
            {
                return Result<IEnumerable<ScheduleWorkoutDto>>.Failure(getUserResult.Error);
            }
            IEnumerable<ScheduleWorkout> scheduleWorkouts = await _unitOfWork.scheduleWorkouts.GetScheduleWorkouts(getUserResult.Values);
            IEnumerable<ScheduleWorkoutDto> scheduleWorkoutsDto = _mapper.Map<IEnumerable<ScheduleWorkoutDto>>(scheduleWorkouts);
            return Result<IEnumerable<ScheduleWorkoutDto>>.Success(scheduleWorkoutsDto);

        }

        public async Task<Result<string>> SetWorkoutSchedule(ScheduleWorkoutDto model, ClaimsPrincipal user)
        {
            Result<Guid> getUserResult = _authService.GetUserId(user);
            if (getUserResult.IsFailure)
            {
                return Result<string>.Failure(getUserResult.Error);
            }
            Result<WorkoutPlan> accessResult = await _workoutPlanService.CheckAccess(model.WorkoutId, getUserResult.Values);
            if (accessResult.IsFailure)
            {
                return Result<string>.Failure(accessResult.Error);
            }

            if (model.ScheduledDate < DateTime.Today) return Result<string>.Failure(ScheduleWorkoutError.InvalidDate);

            ScheduleWorkout scheduleWorkout = ScheduleWorkout.Create(model.ScheduledDate,model.WorkoutId);
            await _unitOfWork.scheduleWorkouts.Add(scheduleWorkout);
            await _unitOfWork.Save();

            return Result<string>.Success("Workout scheduled successfully.");

        }

        public async Task<Result<string>> UpdateScheduledWorkout(ScheduleWorkoutDto model, ClaimsPrincipal user)
        {
            Result<Guid> getUserResult = _authService.GetUserId(user);
            if (getUserResult.IsFailure)
            {
                return Result<string>.Failure(getUserResult.Error);
            }
            Result<ScheduleWorkout> accessResult = await CheckAccess(model.Id, getUserResult.Values);
            if (accessResult.IsFailure)
            {
                return Result<string>.Failure(accessResult.Error);
            }

            if (model.ScheduledDate < DateTime.Today) return Result<string>.Failure(ScheduleWorkoutError.InvalidDate);

            ScheduleWorkout existingSchedule = accessResult.Values;
            existingSchedule.ScheduledDate = model.ScheduledDate;
            if (model.WorkoutId != Guid.Empty)
            {
                existingSchedule.WorkoutId = model.WorkoutId;
            }

            _unitOfWork.scheduleWorkouts.Update(existingSchedule);
            await _unitOfWork.Save();

            return Result<string>.Success("Scheduled workout updated successfully.");

        }
        private async Task<Result<ScheduleWorkout>> CheckAccess(Guid? sheduleWorkouId, Guid userId)
        {
            ScheduleWorkout scheduleWorkout = await _unitOfWork.scheduleWorkouts.Get(wp => wp.Id == sheduleWorkouId
                                               && wp.Workout.UserId == userId);
            if (scheduleWorkout == null)
            {
                return Result<ScheduleWorkout>.Failure(ScheduleWorkoutError.ScheduleNotFound);
            }
            return Result<ScheduleWorkout>.Success(scheduleWorkout);
        }

        public async Task<Result<string>> CompleteWorkout(Guid scheduleWorkoutId, ClaimsPrincipal user)
        {
            Result<Guid> getUserResult = _authService.GetUserId(user);
            if (getUserResult.IsFailure) return Result<string>.Failure(getUserResult.Error);
            var schedule = await _unitOfWork.scheduleWorkouts.Get(s => s.Id == scheduleWorkoutId && s.Workout.UserId == getUserResult.Values);
            if (schedule == null) return Result<string>.Failure(ScheduleWorkoutError.ScheduleNotFound);
            schedule.IsCompleted = true;
            _unitOfWork.scheduleWorkouts.Update(schedule);
            await _unitOfWork.Save();
            return Result<string>.Success("Congratulations! You have completed the training session.");
        }

    }
}
