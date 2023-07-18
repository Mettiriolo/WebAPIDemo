using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using WebAPIDemo.EF;
using WebAPIDemo.Filters;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FilterTestController : Controller
    {
        private MyDbContext _db;

        public FilterTestController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult FilterTest()
        {
            Console.WriteLine("Action执行中");
            throw new NotImplementedException();
        }

        [HttpPost]
        [NotTransactional]
        public async Task<IActionResult> TransTest()
        {
            //using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    await _db.Books.AddAsync(new Book { Name = ".Net Proggrammer", Price = 10.05m, PubDate = DateTime.Parse("2022-12-22") });
            //    await _db.SaveChangesAsync();
            //    await _db.Persons.AddAsync(new Person { Name = "L", Age = 18, Sex = Sexes.Man });
            //    await _db.SaveChangesAsync();
            //    transactionScope.Complete();
            //}
            await _db.Books.AddAsync(new Book { Name = ".Net Proggrammer", Price = 10.05m, PubDate = DateTime.Parse("2022-12-22") });
            await _db.SaveChangesAsync();
            await _db.Persons.AddAsync(new Person { Name = "Lqqq", Age = 18, Sex = Sexes.Man });
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
