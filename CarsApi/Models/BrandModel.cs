using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Models
{
    public class BrandModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IPublishedContent? Icon { get; set; }
        public string Url { get; set; } = null!;
        public DateTime FoundationDate { get; set; }
        public string OriginCountry { get; set; } = null!;
        public string Desctiption { get; set; } = null!;

    }
}
