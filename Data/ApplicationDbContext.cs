using Microsoft.EntityFrameworkCore;
using Notification.API.Models;

namespace Notification.API.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<DbContext> options):base(options)
        {

        }
        
         protected override void OnModelCreating(ModelBuilder builder)
         {
             
         }
        public DbSet<OrderModel> OrderDetails{get;set;}
        
    }
}