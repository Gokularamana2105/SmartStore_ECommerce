using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartStoreData.Data;
using SmartStoreData.IRepositories;
using SmartStoreData.Repositories;
using Microsoft.AspNetCore.Identity;
using SmartStoreModelsUtility;
using Microsoft.AspNetCore.Identity.UI.Services;
using SmartStoreModels.Models.BaseModels;
using Stripe;


namespace SmartStoreProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<AppDbContext>(
                o =>
                {
                    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                 
                });

            builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IOrderSummaryRepository, OrderSummaryRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripeSettings")["SecretKey"];

            builder.Services.ConfigureApplicationCookie(o =>
            {
                o.LogoutPath= $"/Identity/Account/Logout";
                o.LoginPath= $"/Identity/Account/Login";
                o.AccessDeniedPath= $"/Identity/Account/AccessDenied";
            });

            #region Seeding Data
            async void Update(IHost host)
            {
                var scope=host.Services.CreateScope();
                using (var services = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    if (services.Database.IsSqlServer())
                    {
                        services.Database.Migrate();
                    }
                    await SeedData.SeedCategory(services);
                }
            }
            #endregion
            var app = builder.Build();
             Update(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
