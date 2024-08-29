using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationPerformance.Data.Entities;

namespace WebApplicationPerformance.Data
{
    public class LibraryDBContext : DbContext
    {
        public LibraryDBContext(DbContextOptions<LibraryDBContext>options) : base(options) { }
        public virtual DbSet<Book> Book { get; set; }
    }
}
