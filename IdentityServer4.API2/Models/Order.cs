namespace IdentityServer4.API2.Models
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; init; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; init; }
        public List<OrderItem> OrderItems { get; set; }
    }
}