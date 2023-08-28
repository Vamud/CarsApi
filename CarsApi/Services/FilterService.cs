using CarsApi.Models;
using CarsApi.Models.Request;
using CarsApi.Models.Response;
using CarsApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace CarsApi.Services
{
    public class FilterService : IFilterService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly ServiceContext _serviceContext;
        public FilterService(UmbracoHelper umbracoHelper,
            IVariationContextAccessor variationContextAccessor,
            ServiceContext context)
        {
            _umbracoHelper = umbracoHelper;
            _variationContextAccessor = variationContextAccessor;
            _serviceContext = context;
        }
        public IndexViewModel Filtered(FilteredItemsRequest request, IEnumerable<IPublishedContent> nodes, IPublishedContent currentPage)
        {
            var settings = _umbracoHelper.ContentSingleAtXPath("//settings")!;
            var pageSize = settings.Value<int>("pageSize");
            var defImg = settings.Value<IPublishedContent>("defaultImage");

            var carModels = new List<CarModel>();

            foreach (var item in nodes)
            {
                var models = item.Children().Select(c =>
                new CarModel
                {
                    Name = c.Name,
                    BrandId = c.Ancestor()!.Id,
                    Image = c.Value<IPublishedContent>("image") ?? defImg,
                    LaunchDate = c.Value<DateTime>("launchDate"),
                    Url = c.Url(),
                    Description = c.Value<string>("description")!,
                    Price = c.Value<decimal>("price")
                }).ToList();
                carModels.AddRange(models);
            }

            var brandOptions = nodes.Select(b => new IndexBrandModel
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();

            brandOptions.Insert(0, new IndexBrandModel { Id = null, Name = _umbracoHelper.GetDictionaryValue("All")! });

            var brands = new SelectList(brandOptions, "Id", "Name", request.Brand);

            var priceOptions = GetPriceOptionsByCarModels(carModels);
            var minPrices = new SelectList(priceOptions, "MinPrice", "TextMinPrice", request.MinPrice);
            var maxPrices = new SelectList(priceOptions, "MaxPrice", "TextMaxPrice", request.MaxPrice);

            var yearOptions = GetYearOptionsByCarModels(carModels);
            var minYears = new SelectList(yearOptions, "MinYear", "MinYearText", request.MinYear);
            var maxYears = new SelectList(yearOptions, "MaxYear", "MaxYearText", request.MaxYear);

            carModels = FilterByBrand(carModels, request.Brand);
            carModels = FilterByPrice(carModels, request.MinPrice, request.MaxPrice);
            carModels = FilterByYear(carModels, request.MinYear, request.MaxYear);

            var totalPages = (int)Math.Ceiling(carModels.Count() / (double)pageSize);
            var items = carModels.Skip((request.Page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new IndexViewModel(currentPage, new PublishedValueFallback(_serviceContext, _variationContextAccessor))
            {
                CarModels = items,
                PageViewModel = new PageViewModel
                {
                    PageNumber = request.Page,
                    TotalPages = totalPages
                },
                FilterViewModel = new FilterViewModel
                {
                    Brands = brands,
                    SelectedBrand = request.Brand,
                    MinPrices = minPrices,
                    SelectedMinPrice = request.MinPrice,
                    MaxPrices = maxPrices,
                    SelectedMaxPrice = request.MaxPrice,
                    MinYears = minYears,
                    SelectedMinYear = request.MinYear,
                    MaxYears = maxYears,
                    SelectedMaxYear = request.MaxYear
                },
            };



            return viewModel;
        }

        public FilteredCarModelResponse FilteredContent(string culture, int page, int? brand, int? minPrice, int? maxPrice, int? minYear, int? maxYear)
        {
            _variationContextAccessor.VariationContext = new VariationContext(culture[0..5]);
            var rootNode = _umbracoHelper.ContentSingleAtXPath("//cars")!;
            var nodes = rootNode.Children();

            var settings = _umbracoHelper.ContentSingleAtXPath("//settings")!;
            var pageSize = settings.Value<int>("pageSize");
            var defImg = settings.Value<IPublishedContent>("defaultImage")!.GetCropUrl(height: 250, width: 400)!;

            var carModels = new List<CarModel>();

            foreach (var item in nodes)
            {
                var models = item.Children().Select(c =>
                new CarModel
                {
                    Id = c.Key,
                    Name = c.Name,
                    BrandId = c.Ancestor()!.Id,
                    ImageUrl = c.Value<IPublishedContent>("image") != null ? c.Value<IPublishedContent>("image")!.GetCropUrl(height: 250, width: 400)! : defImg,
                    LaunchDate = c.Value<DateTime>("launchDate"),
                    Url = c.Url(),
                    Description = c.Value<string>("description")!,
                    Price = c.Value<decimal>("price")
                }).ToList();
                carModels.AddRange(models);
            }

            var brandOptions = nodes.Select(b => new IndexBrandModel
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();

            brandOptions.Insert(0, new IndexBrandModel { Id = null, Name = _umbracoHelper.GetDictionaryValue("All")! });

            var priceOptions = GetPriceOptionsByCarModels(carModels);

            var yearOptions = GetYearOptionsByCarModels(carModels);

            carModels = FilterByBrand(carModels, brand);
            carModels = FilterByPrice(carModels, minPrice, maxPrice);
            carModels = FilterByYear(carModels, minYear, maxYear);

            var totalPages = (int)Math.Ceiling(carModels.Count() / (double)pageSize);
            var items = carModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new FilteredCarModelResponse
            {
                CarModels = items,
                PageNumber = page,
                TotalPages = totalPages,
                SelectedBrand = brand,
                BrandOptions = brandOptions,
                SelectedMinPrice = minPrice,
                PriceOptions = priceOptions,
                SelectedMaxPrice = maxPrice,
                YearOptions = yearOptions,
                SelectedMinYear = minYear,
                SelectedMaxYear = maxYear
            };

            return result;
        }


        private List<CarModel> FilterByBrand(IEnumerable<CarModel> models, int? brand)
        {
            if (brand == null)
            {
                return models.ToList();
            }

            models = models.Where(c => c.BrandId == brand);
            return models.ToList();
        }
        private List<CarModel> FilterByPrice(IEnumerable<CarModel> models, int? minPrice, int? maxPrice)
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
        private List<CarModel> FilterByYear(IEnumerable<CarModel> models, int? minYear, int? maxYear)
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
        private List<PriceFilterModel> GetPriceOptionsByCarModels(List<CarModel> models)
        {
            var minPrice = models.Select(c => (int)c.Price).Min();
            var maxPrice = models.Select(c => (int)c.Price).Max();

            var priceOptions = new List<PriceFilterModel>
            {
                new PriceFilterModel
                {
                    MinPrice = null,
                    MaxPrice = null,
                    TextMinPrice = _umbracoHelper.GetDictionaryValue("Min")!,
                    TextMaxPrice = _umbracoHelper.GetDictionaryValue("Max")!
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
        private List<YearFilterModel> GetYearOptionsByCarModels(List<CarModel> models)
        {
            var minYear = models.Select(c => c.LaunchDate.Year).Min();
            var maxYear = models.Select(c => c.LaunchDate.Year).Max();

            var yearOptions = new List<YearFilterModel>
            {
                new YearFilterModel
                {
                    MinYear = null,
                    MaxYear = null,
                    MinYearText = _umbracoHelper.GetDictionaryValue("Min")!,
                    MaxYearText = _umbracoHelper.GetDictionaryValue("Max")!
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
