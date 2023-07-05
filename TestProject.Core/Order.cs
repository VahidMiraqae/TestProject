namespace TestProject.Core
{
    public class Order
    {



        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public double Discount { get; set; }
        public decimal Total { get; private set; }
        public decimal DiscountedTotal { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        private Order()
        {
            
        }

        public static Order NewOrder()
        {
            return new Order()
            {
                OrderId = Guid.NewGuid(),
            };
        }

        public void CalculateTotal()
        {
            Total = OrderItems.Select(aa => aa.Quantity * aa.UnitPrice).Sum();
            DiscountedTotal = (decimal)(1 - Discount) * Total;
        }
    }
}
