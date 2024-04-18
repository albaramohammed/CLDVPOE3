using System.Collections.Generic;

namespace CLDV1.Models.ViewModel
{
    public class SearchViewModel
    {
        public string SearchQuery { get; set; }
        public List<ProductViewModel> SearchResults { get; set; }
        public long TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string ErrorMessage { get; set; }

        public SearchViewModel()
        {
            SearchResults = new List<ProductViewModel>();
        }
    }
}