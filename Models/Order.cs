using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be postive.")]
        public double TotalPrice { get; set; }

        public string OrderStatus { get; set; }

        [Required]  
        public int BuyerId { get; set; }

        [ForeignKey("BuyerId")]
        public User Buyer { get; set; }

        [Required]
        public int PetId { get; set; }
        public Pet Pet { get; set; }

    }
}
