using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
