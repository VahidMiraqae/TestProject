namespace TestProject.Application.Dtos
{
    public class PlaceOrderDto
    {
        public Guid UserId { get; set; }
        public PlaceOrderItemDto[] OrderItems { get; set; }
    }
}
