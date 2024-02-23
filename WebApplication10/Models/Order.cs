namespace WebApplication10.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
