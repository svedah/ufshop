using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ufshop.Components;
using ufshop.Components.Account;
using ufshop.Data;
using ufshop.Data.Seed;
using ufshop.Services;

namespace ufshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddControllers(); // for swish callback

            builder.Services.AddBlazorBootstrap();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            builder.Services.AddScoped<BeService, BeService>();

            // builder.Services.AddAuthentication(options =>
            //     {
            //         options.DefaultScheme = IdentityConstants.ApplicationScheme;
            //         options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //     })
            //     .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //     .AddEntityFrameworkStores<ApplicationDbContext>()
            //     .AddSignInManager()
            //     .AddDefaultTokenProviders();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            // builder.Services.AddSession(options =>
            // {
            //     options.IdleTimeout = TimeSpan.FromSeconds(3600);
            //     options.Cookie.HttpOnly = true;
            //     options.Cookie.IsEssential = true;
            // });
            builder.Services.AddDistributedMemoryCache(); // Required for session storage
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1440); // Session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });


            var app = builder.Build();

            //Trigger seeding
            using (var scope = app.Services.CreateScope())
            {
                // var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var context = scope.ServiceProvider.GetRequiredService<BeService>();
                new Seeder(context).Seed();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();

            app.UseAntiforgery();
            app.UseStaticFiles();
            // app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.MapControllers();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.UseSession();

            app.Run();
        }
    }
}
