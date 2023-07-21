using Bogus;
using CarsApi.Models;
using CarsApi.Services.Interfaces;

namespace CarsApi.Services
{
    public class FilterSevice : IFilterService
    {
        public List<CarModel> FilterByBrand(IEnumerable<CarModel> models, int? brand)
        {
            if (brand == null)
            {
                return models.ToList();
            }

            models = models.Where(c => c.BrandId == brand);
            return models.ToList();
        }
        public List<CarModel> FilterByPrice(IEnumerable<CarModel> models, int? minPrice, int? maxPrice)
        {
           
            if (minPrice != null)
            {
                models = models.Where(c => (int)c.Price > minPrice);
            }

            if (maxPrice != null)
            {
                models = models.Where(c => (int)c.Price < maxPrice);
            }

            return models.ToList();
        }
        public List<CarModel> FilterByYear(IEnumerable<CarModel> models, int? minYear, int? maxYear)
        {
            if (minYear != null)
            {
                models = models.Where(c => c.LaunchDate.Year > minYear);
            }

            if (maxYear != null)
            {
                models = models.Where(c => c.LaunchDate.Year < maxYear);
            }

            return models.ToList();
        }
        public  List<PriceFilterModel> GetPriceOptionsByCarModels(List<CarModel> models)
        {
            var minPrice = models.Select(c => (int)c.Price).Min();
            var maxPrice = models.Select(c => (int)c.Price).Max();

            var priceOptions = new List<PriceFilterModel>
            {
                new PriceFilterModel
                {
                    MinPrice = null,
                    MaxPrice = null,
                    TextMinPrice = "Min",
                    TextMaxPrice = "Max"
                }
            };

            while (minPrice < maxPrice)
            {
                minPrice += 10000;
                priceOptions.Add(new PriceFilterModel
                {
                    MinPrice = minPrice,
                    MaxPrice = minPrice,
                    TextMinPrice = minPrice.ToString(),
                    TextMaxPrice = minPrice.ToString(),
                });
            }

            return priceOptions;
        }
        public List<YearFilterModel> GetYearOptionsByCarModels(List<CarModel> models)
        {
            var minYear = models.Select(c => c.LaunchDate.Year).Min();
            var maxYear = models.Select(c => c.LaunchDate.Year).Max();

            var yearOptions = new List<YearFilterModel>
            {
                new YearFilterModel
                {
                    MinYear = null,
                    MaxYear = null,
                    MinYearText = "Min",
                    MaxYearText = "Max"
                }
            };

            while (minYear < maxYear)
            {
                minYear += 3;
                yearOptions.Add(new YearFilterModel
                {
                    MinYear = minYear,
                    MaxYear = minYear,
                    MinYearText = minYear.ToString(),
                    MaxYearText = minYear.ToString()
                });
            }

            return yearOptions;
        }
    }
}
