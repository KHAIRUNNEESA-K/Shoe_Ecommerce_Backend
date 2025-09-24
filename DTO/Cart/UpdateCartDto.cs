namespace ONSTEPS_API.DTO.Cart
{
    public class UpdateCartDto
    {
        public string Product_Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Size { get; set; }
        public string Image_Url { get; set; }
        public string CategoryName { get; set; }
        public int TotalQuantity { get; set; }
    }
}
