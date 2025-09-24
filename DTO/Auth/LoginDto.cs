using System.ComponentModel.DataAnnotations;

namespace ONSTEPS_API.DTO.Auth
{
    public class LoginDto
    {
        [Required]
        [StringLength(10)]
        public string UserName { get; set; }
        [Required]
        [StringLength(8)]
        public string Password { get; set; }
    }
}
