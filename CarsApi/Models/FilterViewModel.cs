using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarsApi.Models
{
    public class FilterViewModel
    {
        public required SelectList Brands { get; set; }
        public required int? SelectedBrand { get; set; }
        public required SelectList MinPrices { get; set; }
        public required int? SelectedMinPrice { get; set; }
        public required SelectList MaxPrices { get; set; }
        public required int? SelectedMaxPrice { get; set; }
        public required SelectList MinYears { get; set; }
        public required int? SelectedMinYear { get; set; }
        public required SelectList MaxYears { get; set; }
        public required int? SelectedMaxYear { get; set; }
    }
}
