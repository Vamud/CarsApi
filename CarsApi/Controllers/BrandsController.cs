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
    public class BrandsController : RenderController
    {
        private readonly UmbracoHelper _umbracoHelper;
        public BrandsController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            UmbracoHelper umbracoHelper)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _umbracoHelper = umbracoHelper;
        }

        [HttpGet]
        public override IActionResult Index()
        {
            var rootNode = _umbracoHelper.ContentSingleAtXPath("//cars");
            var nodes = rootNode!.Children();

            var defImg = _umbracoHelper.ContentSingleAtXPath("//settings")!.Value<IPublishedContent>("defaultImage");

            var brands = nodes.Select(b => new BrandModel
            {
                Id = b.Id,
                Name = b.Name,
                Icon = b.Value<IPublishedContent>("icon") ?? defImg,
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
