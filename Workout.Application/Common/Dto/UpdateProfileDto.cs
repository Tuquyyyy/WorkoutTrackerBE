using System.ComponentModel.DataAnnotations;

namespace Workout.Application.Common.Dto
{
    public class UpdateProfileDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
