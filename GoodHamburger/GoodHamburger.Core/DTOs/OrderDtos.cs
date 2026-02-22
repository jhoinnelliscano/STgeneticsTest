namespace GoodHamburger.Core.DTOs
{
    public class SandwichDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class ExtraDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class OrderRequestDto
    {
        public int SandwichId { get; set; }
        public List<int> ExtraIds { get; set; } = [];
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string SandwichName { get; set; } = string.Empty;
        public List<string> Extras { get; set; } = [];
        public decimal TotalBeforeDiscount { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
