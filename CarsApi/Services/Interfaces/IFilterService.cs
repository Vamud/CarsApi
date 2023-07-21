using CarsApi.Models;

namespace CarsApi.Services.Interfaces
{
    public interface IFilterService
    {
        List<CarModel> FilterByBrand(IEnumerable<CarModel> models, int? brand);
        List<CarModel> FilterByPrice(IEnumerable<CarModel> models, int? minPrice, int? maxPrice);
        List<CarModel> FilterByYear(IEnumerable<CarModel> models, int? minYear, int? maxYear);
        List<PriceFilterModel> GetPriceOptionsByCarModels(List<CarModel> models);
        List<YearFilterModel> GetYearOptionsByCarModels(List<CarModel> models);
    }
}
