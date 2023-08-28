using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Models.Response
{
    public class CarModelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BrandId { get; set; }
        public string Image { get; set; } = null!;
        public string Url { get; set; } = null!;
        public DateTime LaunchDate { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
