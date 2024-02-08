namespace AWS.Core.Entities
{
    public class OrderLine
    {
        public OrderLine(string title, int qty, decimal price)
        {
            Title = title;
            Qty = qty;
            Price = price;
        }

        public string Title { get; private set; }
        public int Qty { get; private set; }
        public decimal Price { get; private set; }
    }
}