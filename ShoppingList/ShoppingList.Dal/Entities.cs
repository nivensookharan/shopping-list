using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingList.Dal.Mapping;
using ShoppingList.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ShoppingList.Dal
{

    public class Entities : DbContext
    {
        public Entities() : base()
        {
            
        }

        public Entities(DbContextOptions<Entities> options) : base()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=192.168.10.202;Database=ShoppingList;User ID=sa; Password=XJyzH9HSZiipKSR0; TrustServerCertificate=True");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingList.Domain.ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());


        }
    }
}
