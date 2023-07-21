namespace CarsApi.Models
{
    public class IndexViewModel
    {
        public required IEnumerable<CarModel> CarModels { get; set; }
        public required PageViewModel PageViewModel { get; set; }
        public required FilterViewModel FilterViewModel { get; set; }
    }
}
