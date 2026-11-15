using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class PerformedExercise
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Wybierz typ ćwiczenia.")]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Typ ćwiczenia")]
        public ExerciseType? ExerciseType { get; set; }

        [Required(ErrorMessage = "Wybierz sesję treningową.")]
        [Display(Name = "Sesja treningowa")]
        public int TrainingSessionId { get; set; }

        [Display(Name = "Sesja treningowa")]
        public TrainingSession? TrainingSession { get; set; }

        [Required(ErrorMessage = "Podaj obciążenie.")]
        [Range(0, 1000, ErrorMessage = "Obciążenie musi mieścić się w przedziale od 0 do 1000 kg.")]
        [Display(Name = "Obciążenie (kg)", Description = "Ciężar użyty podczas ćwiczenia w kilogramach")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Podaj liczbę serii.")]
        [Range(1, 100, ErrorMessage = "Liczba serii musi wynosić od 1 do 100.")]
        [Display(Name = "Liczba serii")]
        public int Sets { get; set; }

        [Required(ErrorMessage = "Podaj liczbę powtórzeń.")]
        [Range(1, 500, ErrorMessage = "Liczba powtórzeń w serii musi wynosić od 1 do 500.")]
        [Display(Name = "Liczba powtórzeń w serii")]
        public int Repetitions { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Użytkownik")]
        public IdentityUser? User { get; set; }
    }
}