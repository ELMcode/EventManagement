using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EventManagement.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est requise")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date est requise")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Le lieu est requis")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nombre maximum de participants est requis")]
        [Range(1, int.MaxValue, ErrorMessage = "Le nombre maximum de participants doit être supérieur à 0")]
        public int MaxParticipants { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être supérieur ou égal à 0")]
        public decimal Price { get; set; }

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        public bool IsFull => Registrations.Count >= MaxParticipants;
        public bool HasStarted => Date <= DateTime.Now;
        public bool IsFinished => Date.AddHours(1) <= DateTime.Now;  // On considère qu'un événement dure 1 heure par défaut
    }
}
