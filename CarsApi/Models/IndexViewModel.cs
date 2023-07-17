namespace CarsApi.Models
{
    public class IndexViewModel
    {
        public IEnumerable<CarModel> CarModels { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public IndexViewModel(IEnumerable<CarModel> carModels, PageViewModel pageViewModel, FilterViewModel filterViewModel)
        {
            CarModels = carModels;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
        }
    }
}
