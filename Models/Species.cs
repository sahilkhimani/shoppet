using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Models
{
    public class Species
    {
        [Key]
        public int SpeciesId { get; set; }

        [Required(ErrorMessage = "Please Enter the species name")]
        public string SpeciesName { get; set; }

        public ICollection<Breed> Breeds { get; set; }
    }
}
