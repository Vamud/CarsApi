using Umbraco.Cms.Core.Exceptions;
using Umbraco.Cms.Web.BackOffice.Trees;

namespace CarsApi.Models
{
    public class PageViewModel
    {
        public required int PageNumber { get; set; }
        public required int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
