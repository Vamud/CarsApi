using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Models
{
    public class BrandsViewModel : PublishedContentWrapped
    {
        public BrandsViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
        {
        }

        public List<BrandModel> Brands { get; set; } = null!;
    }
}
