using CarsApi.Models;
using CarsApi.Models.Request;
using CarsApi.Services.Interfaces;
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
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IFilterService _filterService;
        private readonly IVariationContextAccessor _variationContextAccessor;
        public CarsController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            UmbracoHelper umbracoHelper,
            IFilterService filterService,
            IVariationContextAccessor variationContextAccessor
            )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _umbracoHelper = umbracoHelper;
            _filterService = filterService;
            _variationContextAccessor = variationContextAccessor;
        }

        [HttpGet]
        public IActionResult Index(FilteredItemsRequest request)
        {
            const string culture = "en";

            _variationContextAccessor.VariationContext = new VariationContext(culture);

            var culturedRootNode = CurrentPage.Root();

            TempData.Add("CulturedRootNode", culturedRootNode);

            int pageSize = 6;
            var defImg = _umbracoHelper.ContentSingleAtXPath("//settings")!.Value<IPublishedContent>("defaultImage");
            var rootNode = _umbracoHelper.ContentSingleAtXPath("//cars");
            var nodes = rootNode!.Children();

            var carModels = new List<CarModel>();

            foreach ( var item in nodes )
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
            var priceOptions = _filterService.GetPriceOptionsByCarModels(carModels);
            var yearOptions = _filterService.GetYearOptionsByCarModels(carModels);

            carModels = _filterService.FilterByBrand(carModels, request.Brand);
            carModels = _filterService.FilterByPrice(carModels, request.MinPrice, request.MaxPrice);
            carModels = _filterService.FilterByYear(carModels, request.MinYear, request.MaxYear);

            var totalPages = (int)Math.Ceiling(carModels.Count() / (double)pageSize);
            var items = carModels.Skip((request.Page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new IndexViewModel
            {
                CarModels = items,
                PageViewModel = new PageViewModel
                {
                    PageNumber = request.Page,
                    TotalPages = totalPages
                },
                FilterViewModel = new FilterViewModel(brandOptions, request.Brand, priceOptions, request.MinPrice, request.MaxPrice, yearOptions, request.MinYear, request.MaxYear)
            };

            return View("Cars", viewModel);
        }
    }
}
