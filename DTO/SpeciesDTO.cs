using System.ComponentModel.DataAnnotations;

namespace shoppetApi.DTO
{
    public class SpeciesDTO
    {
        [Required(ErrorMessage = "Please Enter the species name")]
        [StringLength(int.MaxValue, MinimumLength = 2)]
        public string SpeciesName { get; set; }
    }
}
