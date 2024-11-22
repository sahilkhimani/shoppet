using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class BreedDTO
    {
        [Required(ErrorMessage = "Please Enter the Breed Name")]
        public string BreedName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select the Species")]
        public int SpeciesId { get; set; }
    }
}
