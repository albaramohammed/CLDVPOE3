using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using CLDV1.Models;
using CLDV1.Models.ViewModel;
using System.Linq;
using CLDV1.Models.CLDV1.Models;

namespace CLDV1.Services
{
    public class SearchService
    {
        private readonly SearchIndexClient _adminClient;
        private readonly SearchClient _searchClient;

        public SearchService(IConfiguration configuration)
        {
            string searchServiceEndPoint = configuration["SearchServiceEndPoint"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];
            string indexName = configuration["SearchIndexName"];

            if (string.IsNullOrEmpty(searchServiceEndPoint) || string.IsNullOrEmpty(adminApiKey) || string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentException("Search service configuration is incomplete. Please check your appsettings.json file.");
            }

            _adminClient = new SearchIndexClient(new Uri(searchServiceEndPoint), new AzureKeyCredential(adminApiKey));
            _searchClient = new SearchClient(new Uri(searchServiceEndPoint), indexName, new AzureKeyCredential(adminApiKey));
        }

        public async Task<SearchViewModel> SearchProductsAsync(string searchText, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return new SearchViewModel
                {
                    SearchQuery = searchText,
                    SearchResults = new List<ProductViewModel>(),
                    TotalCount = 0,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            var options = new SearchOptions
            {
                IncludeTotalCount = true,
                Skip = (page - 1) * pageSize,
                Size = pageSize
            };

            options.Select.Add("ProductId");
            options.Select.Add("Name");
            options.Select.Add("Description");
            options.Select.Add("Price");
            options.Select.Add("ImagePath");
            options.Select.Add("Category");

            var searchResponse = await _searchClient.SearchAsync<Product>(searchText, options);
            var searchResults = searchResponse.Value;

            var productViewModels = searchResults.GetResults().Select(r => new ProductViewModel
            {
                ProductId = r.Document.ProductId,
                Name = r.Document.Name,
                Description = r.Document.Description,
                Price = r.Document.Price,
                ImagePath = r.Document.ImagePath,
                Category = r.Document.Category
            }).ToList();

            return new SearchViewModel
            {
                SearchQuery = searchText,
                SearchResults = productViewModels,
                TotalCount = searchResults.TotalCount ?? 0,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        // Other methods (CreateIndexAsync, IndexProductsAsync) remain unchanged
    }
}