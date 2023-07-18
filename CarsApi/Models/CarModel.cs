namespace CarsApi.Models
{
	public class CarModel
	{
        public string Name { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string? Image { get; set; } = null;
        public string Url { get; set; } = null!;
        public DateTime LaunchDate { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
