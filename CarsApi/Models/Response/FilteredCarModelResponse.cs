using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarsApi.Models.Response
{
    public class FilteredCarModelResponse
    {
        public required IEnumerable<CarModel> CarModels { get; set; }
        public required int PageNumber { get; set; }
        public required int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public required int? SelectedBrand { get; set; }
        public required List<IndexBrandModel> BrandOptions { get; set; }
        public required int? SelectedMinPrice { get; set; }
        public required List<PriceFilterModel> PriceOptions { get; set; }
        public required int? SelectedMaxPrice { get; set; }
        public required List<YearFilterModel> YearOptions { get; set; }
        public required int? SelectedMinYear { get; set; }
        public required int? SelectedMaxYear { get; set; }
    }
}
