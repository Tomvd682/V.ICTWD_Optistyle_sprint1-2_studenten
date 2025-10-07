using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class ReviewModel
    {
        [Required(ErrorMessage = "Voor- en achternaam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag maximaal 100 tekens bevatten.")]
        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$", ErrorMessage = "Alleen letters en spaties zijn toegestaan.")]
        [Display(Name = "Voor- en achternaam")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Beoordeling is verplicht.")]
        [Range(1, 5, ErrorMessage = "De beoordeling moet tussen 1 en 5 liggen.")]
        [Display(Name = "Beoordeling (1 = slecht, 5 = uitstekend)")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Toelichting is verplicht.")]
        [StringLength(100, ErrorMessage = "De toelichting mag maximaal 100 tekens bevatten.")]
        [Display(Name = "Toelichting")]
        public string Comment { get; set; }
    }
}
