using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // Added this using statement

namespace POL1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure services such as database connections, authentication, etc.

            // Add DbContext configuration here

            services.AddDbContext<YourDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Other services configuration (if any)
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Configure middleware and routing here.

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // If using authentication

            app.UseAuthorization(); // If using authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    // Define YourDbContext class here with DbSet properties for your database tables
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
        {
        }

        // Define DbSet properties for your database tables
      //  public DbSet<FormEntry> FormEntries { get; set; }
        // Add DbSet properties for other tables as needed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here
        }
    }
}

// Define your entity classes for database tables
//public class FormEntry
// {
//    public int Id { get; set; }
    // Define other properties for FormEntry table
    // Example: public string Date { get; set; }
    // Add other properties as needed
// }
