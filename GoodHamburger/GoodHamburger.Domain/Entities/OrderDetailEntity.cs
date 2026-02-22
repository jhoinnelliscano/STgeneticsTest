namespace GoodHamburger.Domain.Entities
{
    public class OrderDetailEntity
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdExtra { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public virtual OrderEntity Order { get; set; } = new OrderEntity();
        public virtual ExtraEntity Extra { get; set; } = new ExtraEntity();

    }
}
