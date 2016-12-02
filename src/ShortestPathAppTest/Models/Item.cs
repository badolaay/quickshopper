namespace QuickShopper.Models
{
    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
    }
}
