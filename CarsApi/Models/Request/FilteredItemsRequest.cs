namespace CarsApi.Models.Request
{
    public class FilteredItemsRequest
    {
        public int? Brand { get; set; } = null;
        public int Page { get; set; } = 1;
        public int? MinPrice { get; set;} = null;
        public int? MaxPrice { get; set; } = null;
        public int? MinYear { get; set; } = null;
        public int? MaxYear { get; set; } = null;
    }
}
