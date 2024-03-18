namespace Web_Asm2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
		public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
		public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
