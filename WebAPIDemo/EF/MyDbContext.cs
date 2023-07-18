using Microsoft.EntityFrameworkCore;

namespace WebAPIDemo.EF
{
    public class MyDbContext:DbContext
    {


        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {
        }
    }
}
