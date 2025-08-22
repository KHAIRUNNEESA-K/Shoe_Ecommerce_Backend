using System.ComponentModel.DataAnnotations;

namespace ONSTEPS_API.DTO
{
    public class AdminResponseDto
    {
      
        public string Name { get; set; }
     
        public string Email { get; set; }
     
        public string PhoneNo { get; set; }
   
        public string UserName { get; set; }
  
        public DateTime CreatedAt { get; set; }
      
        public DateTime UpdatedAt { get; set; }
        public bool IsBlocked { get; set; }

    }
}
