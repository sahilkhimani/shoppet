using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Models
{
    public class Breed
    {
        [Key]
        public int BreedId { get; set; }

        [Required]
        public string BreedName { get; set; }
        
        [Required]
        public int SpeciesId { get; set; }
        public Species Species { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}
