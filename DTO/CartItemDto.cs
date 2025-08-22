namespace ONSTEPS_API.DTO
{
    public class CartItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public decimal Price {  get; set; }
        public int Size {  get; set; }
        public string Image_Url { get; set; }
        public string CategoryName {  get; set; }
        public int Quantity { get; set; }
    }
}
