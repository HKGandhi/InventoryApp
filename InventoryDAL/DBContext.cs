using InventoryDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDAL
{
    public class DBContext : DbContext
    {

        public DBContext(
        DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Invertory> Invertories { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Booking> Bookings { get; set; }

    }
}
