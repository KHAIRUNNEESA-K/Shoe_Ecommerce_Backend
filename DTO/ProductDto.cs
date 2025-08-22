using System.ComponentModel.DataAnnotations;

namespace ONSTEPS_API.DTO
{
    public class ProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Size {  get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Url]
        public string Image_Url { get; set; }

        [Required]
        public int CategoryById { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}

  
