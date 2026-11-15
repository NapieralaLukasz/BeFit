using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data i czas rozpoczęcia są wymagane.")]
        [Display(Name = "Rozpoczęcie sesji", Description = "Data i godzina rozpoczęcia treningu")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Data i czas zakończenia są wymagane.")]
        [Display(Name = "Zakończenie sesji", Description = "Data i godzina zakończenia treningu")]
        public DateTime EndTime { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Użytkownik")]
        public IdentityUser? User { get; set; }
    }
}