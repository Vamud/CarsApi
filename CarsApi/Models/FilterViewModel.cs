using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarsApi.Models
{
    public class FilterViewModel
    {
        public SelectList Brands { get; set; }
        public int? SelectedBrand { get; set; }
        public SelectList MinPrices { get; set; }
        public int? SelectedMinPrice { get; set; }
        public SelectList MaxPrices { get; set; }
        public int? SelectedMaxPrice { get; set; }
        public SelectList MinYears { get; set; }
        public int? SelectedMinYear { get; set; }
        public SelectList MaxYears { get; set; }
        public int? SelectedMaxYear { get; set; }

        public FilterViewModel(List<IndexBrandModel> brands, int? brand, List<PriceFilterModel> prices, int? minPrice, int? maxPrice, List<YearFilterModel> years, int? minYear, int? maxYear)
        {
            brands.Insert(0, new IndexBrandModel { Id = null, Name = "All Brands"});
            Brands = new SelectList(brands, "Id", "Name", brand);
            SelectedBrand = brand;
            MinPrices = new SelectList(prices, "MinPrice", "TextMinPrice", minPrice);
            SelectedMinPrice = minPrice;
            MaxPrices = new SelectList(prices, "MaxPrice", "TextMaxPrice", maxPrice);
            SelectedMaxPrice = maxPrice;
            MinYears = new SelectList(years, "MinYear", "MinYearText", minYear);
            SelectedMinPrice = minYear;
            MaxYears = new SelectList(years, "MaxYear", "MaxYearText", maxYear);
            SelectedMaxYear = maxYear;
        }
    }
}
