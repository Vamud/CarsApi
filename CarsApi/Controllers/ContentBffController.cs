using CarsApi.Models.Response;
using CarsApi.Services.Interfaces;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;

namespace CarsApi.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentBffController : UmbracoApiController 
    {
        private const string headerKey = "Accept-Language";
        private readonly IFilterService _filterService;
        private readonly ILocalizationService _localizationService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IVariationContextAccessor _variationContextAccessor;
        public ContentBffController(IFilterService filterService, ILocalizationService localizationService, UmbracoHelper umbracoHelper, IVariationContextAccessor variationContextAccessor)
        {
            _filterService = filterService;
            _localizationService = localizationService;
            _umbracoHelper = umbracoHelper;
            _variationContextAccessor = variationContextAccessor;
        }

        [HttpGet]
        public IActionResult GetFilteredCarModels(int page, int? brand, int? minPrice, int? maxPrice, int? minYear, int? maxYear)
        {
            Request.Headers.TryGetValue(headerKey, out var headerValue);

            var result = _filterService.FilteredContent(headerValue!, page, brand, minPrice, maxPrice, minYear, maxYear);

            return Ok(result);
        }

        [HttpGet("dictionary")]
        public IActionResult GetDictionary()
        {
            Request.Headers.TryGetValue(headerKey, out var headerValue);

            var cultureIsoCode = headerValue.ToString()[0..5];
            _variationContextAccessor.VariationContext = new VariationContext(cultureIsoCode.ToLower());
            var rootItem = _localizationService.GetRootDictionaryItems().FirstOrDefault();
            var item = _localizationService.GetDictionaryItemById(rootItem!.Id);
            var dictionary = _localizationService.GetDictionaryItemChildren(item!.Key);
            var result = dictionary.Select(i => new DictionaryItemModel { Key = i.ItemKey, Translation = i.Translations.SingleOrDefault(x => x.Language.IsoCode.Equals(cultureIsoCode))!.Value });
            return Ok(result);
        }
    }
}
