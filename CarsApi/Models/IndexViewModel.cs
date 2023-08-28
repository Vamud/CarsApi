using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Models
{
    public class IndexViewModel : PublishedContentWrapped
    {
        public IndexViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
        {
        }

        public required IEnumerable<CarModel> CarModels { get; set; }
        public required PageViewModel PageViewModel { get; set; }
        public required FilterViewModel FilterViewModel { get; set; }
    }
}
