using CarsApi.Models;
using CarsApi.Models.Request;
using CarsApi.Models.Response;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Services.Interfaces
{
    public interface IFilterService
    {
        IndexViewModel Filtered(FilteredItemsRequest request, IEnumerable<IPublishedContent> nodes, IPublishedContent currentPage);
        FilteredCarModelResponse FilteredContent(string culture, int page, int? brand, int? minPrice, int? maxPrice, int? minYear, int? maxYear);
    }
}
