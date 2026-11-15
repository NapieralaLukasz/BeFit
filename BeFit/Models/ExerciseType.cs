using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwa ćwiczenia musi mieć od 2 do 100 znaków.")]
        [Display(Name = "Nazwa ćwiczenia", Description = "Np. Wyciskanie sztangi na ławce płaskiej, Przysiad ze sztangą")]
        public string Name { get; set; } = string.Empty;
    }
}