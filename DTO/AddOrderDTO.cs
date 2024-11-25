using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class AddOrderDTO
    {
        [Required(ErrorMessage = "Please select pet")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select pet")]
        public int PetId { get; set; }
    }
}
