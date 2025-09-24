namespace ONSTEPS_API.DTO.Product
{
    public class ProductUpdateDto
    {
        public string? Product_Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int ?Size { get; set; }
        public int? Quantity { get; set; }
        public string? Image_Url { get; set; }
        public int? CategoryById { get; set; }
        public bool? IsActive { get; set; }
    }
}
