namespace ONSTEPS_API.DTO.Admin
{
    public class UserDto
    {
        public int userId { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public string Status { get; set; }
    }
}
