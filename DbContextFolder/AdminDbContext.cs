using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PD_Store.Models.Auth;
using PD_Store.Models.Order;
using PD_Store.Models.Product;

namespace PD_Store.DbContextFolder
{
    public class AdminDbContext : IdentityDbContext<ApplicationUser>
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {

        }
        //PRODUCT
        public DbSet<Products> Products { get; set; }

        public DbSet<ApplicationAreas> ApplicationAreas { get; set; }

        public DbSet<ProductImages> ProductImages { get; set; }

        //ORDER

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Sales> Sales { get; set; }



    }

}
