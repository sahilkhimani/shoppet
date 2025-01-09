using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class PetDTO
    {
        [Required(ErrorMessage = "Please Enter the name of your pet")]
        public string PetName { get; set; }

        [Required(ErrorMessage = "Please Add the Description of your pet"), MinLength(10)]
        public string petDesc { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Age must be a positive number.")]
        public int PetAge { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be postive.")]
        public double PetPrice { get; set; }

        [Required]
        public string PetGender { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BreedId { get; set; }

    }
}
