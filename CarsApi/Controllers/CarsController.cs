using CarsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;

namespace CarsApi.Controllers
{
    public class CarsController : RenderController
    {
        private readonly IPublishedContentQuery _publishedContentQuery;
        private readonly UmbracoHelper _umbracoHelper;
        public CarsController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedContentQuery publishedContentQuery,
            UmbracoHelper umbracoHelper
            )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedContentQuery = publishedContentQuery;
            _umbracoHelper = umbracoHelper; 
        }

        [HttpGet]
        public IActionResult Index(int brand = 0, int page = 1, int minPrice = 0, int maxPrice = 200000, int minYear = 0, int maxYear = 2023)
        {
            
            int pageSize = 6;
            var defImg = _umbracoHelper.Media(Guid.Parse("2ce6128a-c2b9-490c-8507-c167d564ad3f"));
            var rootNode = _publishedContentQuery.Content(1064);
            var nodes = rootNode!.Children();

            var brands = nodes.Select(b => new IndexBrandModel
            {
                Id = b.Id, Name = b.Name
            }).ToList();

            var prices = new List<PriceFilterModel>();
            var years = new List<YearFilterModel>();

            var cars = new List<CarModel>();

            foreach ( var item in nodes )
            {
                var models = item.Children().Select(c =>
				new CarModel
				{
					Name = c.Name,
					BrandName = c.Ancestor()!.Name,
					Image = c.Value<IPublishedContent>("image") ?? defImg,
					LaunchDate = c.Value<DateTime>("launchDate"),
                    Url = c.Url(),
					Description = c.Value<string>("description")!,
					Price = c.Value<decimal>("price")
				}).ToList();
                cars.AddRange(models);
            }

            prices = GetPricesByCarModels(cars);
            years = GetYearsByCarModels(cars);

            if (brand != 0)
            {
                var brandName = brands.FirstOrDefault(b => b.Id == brand)!.Name;
                cars = cars.Where(c => c.BrandName == brandName).ToList();
            }

            if (minPrice > 0)
            {
                cars = cars.Where(c => (int)c.Price > minPrice).ToList();
            }

            if (maxPrice < 200000)
            {
                cars = cars.Where(c => (int)c.Price <  maxPrice).ToList();
            }

            if (minYear > 0)
            {
                cars = cars.Where(c => c.LaunchDate.Year > minYear).ToList();
            }

            if (maxYear < 2023)
            {
                cars = cars.Where(c => c.LaunchDate.Year < maxYear).ToList();
            }

            var count = cars.Count();
            var items = cars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            IndexViewModel viewModel = new IndexViewModel(
                items,
                new PageViewModel(count, page, pageSize),
                new FilterViewModel(brands, brand, prices, minPrice, maxPrice, years, minYear, maxYear)
                );

            return View("Cars", viewModel);
        }

        private List<PriceFilterModel> GetPricesByCarModels(List<CarModel> cars)
        {
            var minPrice = cars.Select(c => (int)c.Price).Min();
            var maxPrice = cars.Select(c => (int)c.Price).Max();

            var prices = new List<PriceFilterModel>
            {
                new PriceFilterModel
                {
                    MinPrice = 0,
                    MaxPrice = 200000,
                    TextMinPrice = "Min",
                    TextMaxPrice = "Max"
                }
            };

            while (minPrice < maxPrice)
            {
                minPrice += 10000;
                prices.Add(new PriceFilterModel
                {
                    MinPrice = minPrice,
                    MaxPrice = minPrice,
                    TextMinPrice = minPrice.ToString(),
                    TextMaxPrice = minPrice.ToString(),
                });
            }

            return prices;
        }

        private List<YearFilterModel> GetYearsByCarModels(List<CarModel> cars)
        {
            var minYear = cars.Select(c => c.LaunchDate.Year).Min();
            var maxYear = cars.Select(c => c.LaunchDate.Year).Max();

            var years = new List<YearFilterModel>
            {
                new YearFilterModel
                {
                    MinYear = 0,
                    MaxYear = 2023,
                    MinYearText = "Min",
                    MaxYearText = "Max"
                }
            };

            while (minYear < maxYear)
            {
                minYear += 3;
                years.Add(new YearFilterModel
                {
                    MinYear = minYear,
                    MaxYear = minYear,
                    MinYearText = minYear.ToString(),
                    MaxYearText = minYear.ToString()
                });
            }

            return years;
        }
    }
}
