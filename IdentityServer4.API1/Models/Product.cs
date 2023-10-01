namespace IdentityServer4.API1.Models
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; init; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; } = 0!;
        public int Stock { get; set; } = 0!;
    }
}