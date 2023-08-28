using Microsoft.Identity.Client;

namespace CarsApi.Models.Response
{
    public class DictionaryItemModel
    {
        public string Key { get; set; } = null!;
        public string Translation { get; set; } = null!;
    }
}
