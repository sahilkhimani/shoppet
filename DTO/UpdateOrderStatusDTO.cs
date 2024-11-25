using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "Please Select the Status")]
        public string OrderStatus { get; set; }
    }
}
