using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Infrastructure.Presistance.Data;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Notes;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.NotesInfo;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Collaborators;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Tags;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using NoteKeeperPro.Application.Common.Services.EmailSettings;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Application.Services.Notes;

namespace NoteKeeperPro.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>((options) =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            });

            // Add Application Services
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<INoteInfoRepository, NoteInfoRepository>();
            builder.Services.AddScoped<IEmailSettings, EmailSettings>();
            builder.Services.AddScoped<INoteService, NoteService>();

            // Configure Identity with ApplicationUser
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure cookie policy
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            });

            var app = builder.Build();

            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while creating the database.");
                }
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Default route - ensure it routes to MainPage/Index
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}" // This routes to MainPage/Index by default
            );

            app.Run();
        }
    }
}
