using Microsoft.AspNetCore.Mvc;
using CLDV1.DataBase;
using CLDV1.Models;
using CLDV1.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CLDV1.Services;
using Microsoft.Extensions.Logging;
using CLDV1.Models.ViewModel;

namespace CLDV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SearchService _searchService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, SearchService searchService, ILogger<HomeController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> MyWork()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                var productViewModels = products.Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    Category = p.Category
                }).ToList();
                return View(productViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Product(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                var productViewModel = new ProductViewModel
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImagePath = product.ImagePath,
                    Category = product.Category
                };
                return View(productViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching product with id {id}.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Cart()
        {
            try
            {
                var cartItems = GetCartItemsFromDatabase();
                var cartItemViewModels = cartItems.Select(item => new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Unknown",
                    Price = item.Product?.Price ?? 0,
                    Quantity = item.Quantity
                }).ToList();
                var cartViewModel = new CartViewModel
                {
                    CartItems = cartItemViewModels,
                    TotalAmount = CalculateTotalAmount(cartItemViewModels)
                };
                return View(cartViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cart items.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Search(string query, int page = 1)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new SearchViewModel());
            }

            try
            {
                var searchResults = await _searchService.SearchProductsAsync(query, page);
                return View(searchResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during search for query: {query}");
                return View(new SearchViewModel
                {
                    SearchQuery = query,
                    SearchResults = new List<ProductViewModel>(),
                    TotalCount = 0,
                    CurrentPage = page,
                    PageSize = 10,
                    ErrorMessage = "An error occurred while performing the search. Please try again later."
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Helper methods
        private List<CartItem> GetCartItemsFromDatabase()
        {
            // Implement logic to retrieve cart items from the database or session
            return new List<CartItem>();
        }

        private decimal CalculateTotalAmount(List<CartItemViewModel> cartItems)
        {
            return cartItems.Sum(item => item.Price * item.Quantity);
        }
    }
}