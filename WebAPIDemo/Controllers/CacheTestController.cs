using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System.Text.Json;
using WebAPIDemo.EF;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CacheTestController : Controller
    {
        private readonly IMemoryCache _memCache;
        private readonly IDistributedCache _distCache;
        private MyDbContext _db;

        public CacheTestController(IMemoryCache memCache, IDistributedCache distCache, MyDbContext db)
        {
            _memCache = memCache;
            _distCache = distCache;
            _db = db;
        }

        [HttpGet]
        [ResponseCache(Duration = 20)]
        public IActionResult ResponseCacheTest()
        {
            string[] names = { "AA", "BB", "CC", "DD", "EE", "FF" };
            return Ok(names);
        }
        [HttpGet]
        public async Task<IActionResult> MemoryCacheTest(int index)
        {
            Book? book = await _memCache.GetOrCreateAsync("Name_" + index, async o =>
            {
                o.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                o.SlidingExpiration = TimeSpan.FromSeconds(5);
                Console.WriteLine("缓存里没有结果");
                return await _db.Books.FirstOrDefaultAsync(x => x.Id == index);
            });
            if (book is null)
                return NotFound();
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> DistributeCacheTest(int index)
        {
            Book? book;
            string? str = await _distCache.GetStringAsync("Name_" + index);
            if (str is null)
            {
                book = await _db.Books.FirstOrDefaultAsync(x => x.Id == index);
                DistributedCacheEntryOptions options = new()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
                    SlidingExpiration = TimeSpan.FromSeconds(5)
                };
            await _distCache.SetStringAsync("Name_" + index, JsonSerializer.Serialize(book),options);
            }
            else
            {
                book = JsonSerializer.Deserialize<Book?>(str);
            }
            if (book is null)
                return NotFound();
            return Ok(book);
        }
    }
}
