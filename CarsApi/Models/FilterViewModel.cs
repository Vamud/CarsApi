using Microsoft.AspNetCore.Mvc.Rendering;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Models
{
    public class FilterViewModel
    {
        public SelectList Brands { get; set; }
        public int SelectedBrand { get; set; }

        public FilterViewModel(List<BrandModel> brands, int brand)
        {
            brands.Insert(0, new BrandModel { Id = 0, Name = "All"});
            Brands = new SelectList(brands, "Id", "Name", brand);
            SelectedBrand = brand;
        }
    }
}
