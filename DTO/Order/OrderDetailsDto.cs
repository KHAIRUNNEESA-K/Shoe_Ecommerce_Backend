namespace ONSTEPS_API.DTO.Order
{
    public class OrderDetailsDto
    {
       
            public OrderDto Order { get; set; }
            public List<OrderItemDto> Items { get; set; }
        
    }
}
