using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopApi.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }

        [Required(ErrorMessage ="Please Enter the name of your pet")]
        public string PetName { get; set; }

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
        public Breed Breed { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        public ICollection<Order> Orders { get; set; }


    }
}
