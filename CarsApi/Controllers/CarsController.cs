using CarsApi.Models;
using Lucene.Net.Analysis.Br;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure;
using Umbraco.Cms.Web.Common.Controllers;

namespace CarsApi.Controllers
{
    public class CarsController : RenderController
    {
        private readonly IPublishedContentQuery _publishedContentQuery;
        public CarsController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedContentQuery publishedContentQuery
            )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedContentQuery = publishedContentQuery;
        }

        [HttpGet]
        public IActionResult Index(int brand = 0, int page = 1)
        {
            int pageSize = 7;

            var rootNode = _publishedContentQuery.Content(1064);
            var nodes = rootNode!.Children();
            var brands = nodes.Select(b => new BrandModel
            {
                Id = b.Id, Name = b.Name
            }).ToList();

            var cars = new List<CarModel>();
            foreach ( var item in nodes )
            {
                var models = item.Children().Select(c =>
				new CarModel
				{
					Name = c.Name,
					BrandName = c.Ancestor()!.Name,
					Image = c.Value<IPublishedContent>("image")!.Url(),
					LaunchDate = c.Value<DateTime>("launchDate"),
                    Url = c.Url(),
					Description = c.Value<string>("description")!,
					Price = c.Value<decimal>("price")
				}).ToList();
                cars.AddRange(models);
            }

            if (brand != 0)
            {
                var brandName = brands.FirstOrDefault(b => b.Id == brand)!.Name;
                cars = cars.Where(c => c.BrandName == brandName).ToList();
            }

            var count = cars.Count();
            var items = cars.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            IndexViewModel viewModel = new IndexViewModel(
                items,
                new PageViewModel(count, page, pageSize),
                new FilterViewModel(brands, brand)
                );

            return PartialView("CarTable", viewModel);
        }
    }
}
