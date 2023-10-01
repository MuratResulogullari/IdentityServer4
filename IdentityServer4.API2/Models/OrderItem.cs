namespace IdentityServer4.API2.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; init; }
        public string PrductName { get; set; }
        public string Description { get; init; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
    }
}