namespace CarsApi.Models
{
    public class YearFilterModel
    {
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public string MinYearText { get; set; } = null!;
        public string MaxYearText { get; set;} = null!;
    }
}
