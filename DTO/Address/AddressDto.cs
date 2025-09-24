namespace ONSTEPS_API.DTO.Address
{
    public class AddressDto
    {
        public int Address_Id { get; set; } 
        public int UserId { get; set; }      
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Pincode { get; set; }
        public string HouseNo { get; set; }
        public string Area { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }  
    }
}
