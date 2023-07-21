namespace CarsApi.Models
{
    public class PriceFilterModel
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string TextMinPrice { get; set; } = null!;
        public string TextMaxPrice { get; set; } = null!;
    }
}
