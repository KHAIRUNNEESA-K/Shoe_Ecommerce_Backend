using System.ComponentModel.DataAnnotations;

namespace ONSTEPS_API.DTO
{
    public class DeleteProductDto
    {
        public int ID {  get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public string Status { get; set; }

      
    }
}
