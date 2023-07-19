using Microsoft.AspNetCore.Mvc.Rendering;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace CarsApi.Models
{
    public class FilterViewModel
    {
        public SelectList Brands { get; set; }
        public int SelectedBrand { get; set; }
        public int SelectedMinPrice { get; set; }
        public int SelectedMaxPrice { get; set; }
        public int SelectedMinYear { get; set; }
        public int SelectedMaxYear { get; set; }

        public FilterViewModel(List<IndexBrandModel> brands, int brand, int maxPrice, int minPrice, int minYear, int maxYear)
        {
            brands.Insert(0, new IndexBrandModel { Id = 0, Name = "All Brands"});
            Brands = new SelectList(brands, "Id", "Name", brand);
            SelectedBrand = brand;
            SelectedMinPrice = minPrice;
            SelectedMaxPrice = maxPrice;
            SelectedMinPrice = minYear;
            SelectedMaxYear = maxYear;
        }
    }
}
