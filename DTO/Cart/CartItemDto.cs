namespace ONSTEPS_API.DTO.Cart
{
    public class CartItemDto
    {
        public string Product_Name { get; set; }
        public string Description { get; set; } 
        public decimal TotalPrice {  get; set; }
        public int Size {  get; set; }
        public string Image_Url { get; set; }
        public string CategoryName {  get; set; }
        public int Quantity { get; set; }
    }
}
