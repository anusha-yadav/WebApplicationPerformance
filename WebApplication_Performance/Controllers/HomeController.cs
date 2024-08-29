using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using WebApplication_Performance.Models;
using Microsoft.AspNetCore.OutputCaching;
using WebApplicationPerformance.Data;
using WebApplicationPerformance.Data.Entities;


namespace WebApplication_Performance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryDBContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly List<Item> _items;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache, LibraryDBContext context)
        {
            _logger = logger;
            _cache = cache;
            _dbContext = context;
            _items = new List<Item>
            {
                new Item {Id = 1,Title = "Don Quixote", Author = "Miguel de Cervantes", ISBN="123-ased-9086" },
                new Item {Id = 2, Title = "Alice's Adventures in Wonderland", Author = "Lewis Carroll", ISBN="123-ased-9086" }
            };
        }

        [ResponseCache(Duration = 10)]
        public IActionResult Index()
        {
            ViewBag.BookCount = _items.Count().ToString();
            ViewBag.CurrentTime = DateTime.Now.ToString();
            return View(_items.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var books = _books;
            var books = _dbContext.Book.ToList();
            return Ok(books);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByRedisCache()
        {
            var cacheKey = "GET_ALL_BOOKS";
            List<Book> books = new();

            var cachedData = await _cache.GetAsync(cacheKey);
            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                books = JsonConvert.DeserializeObject<List<Book>>(cachedDataString);
            }
            else
            {
                //books = _books;
                books = _dbContext.Book.ToList();
                var cachedDataString = JsonConvert.SerializeObject(books);
                var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                var options = new DistributedCacheEntryOptions()
                                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                await _cache.SetAsync(cacheKey, newDataToCache, options);
            }
            return Ok(books);
        }
    }
}
