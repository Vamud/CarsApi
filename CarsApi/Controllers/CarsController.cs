using CarsApi.Models.Request;
using CarsApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;

namespace CarsApi.Controllers
{
    public class CarsController : RenderController
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IFilterService _filterService;
        public CarsController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            UmbracoHelper umbracoHelper,
            IFilterService filterService
            )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _umbracoHelper = umbracoHelper;
            _filterService = filterService;
        }

        [HttpGet]
        public IActionResult Index(FilteredItemsRequest request)
        {
            var rootNode = _umbracoHelper.ContentSingleAtXPath("//cars")!;
            var nodes = rootNode.Children();

            var viewModel = _filterService.Filtered(request, nodes, CurrentPage!);

            return View("Cars", viewModel);
        }
    }
}
