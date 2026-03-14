using System;
using ApiEcommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}
