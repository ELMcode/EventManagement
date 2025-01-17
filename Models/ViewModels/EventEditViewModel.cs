using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ViewModels
{
    public class EventEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [Display(Name = "Nom")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est requise")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date est requise")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Le lieu est requis")]
        [Display(Name = "Lieu")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nombre maximum de participants est requis")]
        [Display(Name = "Nombre maximum de participants")]
        [Range(1, int.MaxValue, ErrorMessage = "Le nombre maximum de participants doit être supérieur à 0")]
        public int MaxParticipants { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Display(Name = "Prix")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être supérieur ou égal à 0")]
        public decimal Price { get; set; }

        public List<EventParticipantViewModel> CurrentParticipants { get; set; } = new List<EventParticipantViewModel>();
        public List<EventParticipantViewModel> AvailableParticipants { get; set; } = new List<EventParticipantViewModel>();
        public List<int>? SelectedParticipantIds { get; set; }
    }

    public class EventParticipantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? RegistrationDate { get; set; }
    }
}
