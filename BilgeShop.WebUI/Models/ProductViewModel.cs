namespace BilgeShop.WebUI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UnitInStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }

    }
}
