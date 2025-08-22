namespace ONSTEPS_API.DTO
{
    public class SearchByCategoryDto
    {
        public string CategoryName { get; set; }
        public int ID {  get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public decimal Price {  get; set; }
        public int Size {  get; set; }
        public int Quantity {  get; set; }
        public string Image_Url {  get; set; }
        public bool IsActive {  get; set; }

    }
}
