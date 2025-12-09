using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ufshop.Data.Models;
using ufshop.Data.Seed;

namespace ufshop.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        // public DbSet<ApplicationUser> Carts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        // public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerInfo> CustomerInfos { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopContactInfo> ShopContactInfos { get; set; }
        public DbSet<ShopImage> ShopImages { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        // public DbSet<ShopItemProperty> ShopItemProperties { get; set; }
        // public DbSet<ShopItemPropertyOption> ShopItemPropertyOptions { get; set; }
        public DbSet<ShopOrder> ShopOrders { get; set; }
        public DbSet<ShopPage> ShopPages { get; set; }
        public DbSet<ShopPageFragment> ShopPageFragments { get; set; }
        public DbSet<ShopSetting> ShopSettings { get; set; }
        public DbSet<ShopSocialMedia> ShopSocialMedias { get; set; }
        public DbSet<UFShopOrder> UFShopOrders { get; set; }

        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        // .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFDataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
        .UseSqlite()
        .UseSeeding((context, _) =>
        {
            // var testBlog = context.Set<Blog>().FirstOrDefault(b => b.Url == "http://test.com");
            // if (testBlog == null)
            // {
            //     context.Set<Blog>().Add(new Blog { Url = "http://test.com" });
            //     context.SaveChanges();
            // }

            // var seeder = new Seeder(context);
            // seeder.Seed();
        });
        // .UseAsyncSeeding(async (context, _, cancellationToken) =>
        // {
        //     var testBlog = await context.Set<Blog>().FirstOrDefaultAsync(b => b.Url == "http://test.com", cancellationToken);
        //     if (testBlog == null)
        //     {
        //         context.Set<Blog>().Add(new Blog { Url = "http://test.com" });
        //         await context.SaveChangesAsync(cancellationToken);
        //     }
        // });
    }
}
