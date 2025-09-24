using System.ComponentModel.DataAnnotations;

namespace ONSTEPS_API.DTO.Auth
{
    public class SignupDto
    {
        [Required]
        [StringLength(50),MinLength(3)]
        public string Name {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(8)]
        public string Password { get; set; }
        [Required]
        [Phone]
        [StringLength(10)]
        public string PhoneNo { get; set; }
        [Required]
        [StringLength (10),MinLength(3)]
        public string UserName { get; set; }

    

    }
}
