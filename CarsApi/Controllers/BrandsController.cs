using CarsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CarsApi.Controllers
{
    public class BrandsController : RenderController
    {
        private readonly IPublishedContentQuery _publishedContentQuery;
        public BrandsController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedContentQuery publishedContentQuery)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedContentQuery = publishedContentQuery;
        }

        [HttpGet]
        public override IActionResult Index()
        {
            var rootNode = _publishedContentQuery.Content(1064);
            var nodes = rootNode!.Children();

            var brands = nodes.Select(b => new BrandModel
            {
                Id = b.Id,
                Name = b.Name,
                Icon = b.Value<IPublishedContent>("icon")?.Url(),
                Url = b.Url(),
                OriginCountry = b.Value<string>("originCountry")!,
                FoundationDate = b.Value<DateTime>("foundationDate"),
                Desctiption = b.Value<string>("description")!
            }).ToList();

            var viewModel = new BrandsViewModel
            {
                Brands = brands
            };

            return View("Brands", viewModel);
        }
    }
}
