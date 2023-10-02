namespace IdentityServer4.WebAppMvc2.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; init; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
    public class OrderItemViewModel
    {

        public string Id { get; init; }
        public string PrductName { get; set; }
        public string Description { get; init; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
    }
}
