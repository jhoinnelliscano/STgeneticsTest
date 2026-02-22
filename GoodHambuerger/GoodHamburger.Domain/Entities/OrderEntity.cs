namespace GoodHamburger.Domain.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int IdSandwich { get; set; }
        public decimal TotalBeforeDiscount { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual SandwichEntity Sandwich { get; set; } = new SandwichEntity();
        public virtual IList<OrderDetailEntity> Details { get; set; } = [];

        public void CalculateTotals()
        {
            TotalBeforeDiscount = Sandwich.Price + Details.Sum(d => d.Extra.Price);
            decimal discountPercentage = CalculateDiscountPercentage(Sandwich, Details.Select(d => d.Extra).ToList());
            Discount = discountPercentage * 100; // Storing as percentage
            Total = TotalBeforeDiscount * (1 - discountPercentage);
        }

        private decimal CalculateDiscountPercentage(SandwichEntity sandwich, IList<ExtraEntity> extras)
        {
            bool hasFries = extras.Any(e => e.Name.Contains("Fries", StringComparison.OrdinalIgnoreCase));
            bool hasDrink = extras.Any(e => e.Name.Contains("Soft drink", StringComparison.OrdinalIgnoreCase));

            if (hasFries && hasDrink) return 0.20m;
            if (hasDrink) return 0.15m;
            if (hasFries) return 0.10m;

            return 0m;
        }
    }
}
