using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Models
{
    public class Breed
    {
        [Key]
        public int BreedId { get; set; }

        [Required(ErrorMessage = "Please Enter the Breed Name")]    
        public string BreedName { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage="Please Select the Species")]
        public int SpeciesId { get; set; }
        public Species Species { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}
