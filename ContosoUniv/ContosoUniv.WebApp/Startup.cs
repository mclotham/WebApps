using ContosoUniv.WebApp.Authorization;
using ContosoUniv.Data;
using ContosoUniv.WebApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace ContosoUniv.WebApp
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddDbContext<ApplicationDbContext>( options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString( "DefaultConnection" ) ) );
            services.AddDbContext<ContosoUnivContext>( options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString( "DefaultConnection" ) ) );
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>( options => options.SignIn.RequireConfirmedAccount = false )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDistributedMemoryCache();

            services.AddSession( options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds( 3600 );
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            } );

            services.AddControllersWithViews();

#if DEBUG
            services.AddRazorPages().AddRazorRuntimeCompilation();
#else
            services.AddRazorPages();
#endif

            services.AddAuthorization( options =>
            {
                foreach ( var claim in Permits.GetAllPermits() )
                    options.AddPolicy( claim, policy => policy.RequireClaim( claim ) );
            } );

            services.AddScoped<IClaimsTransformation, RoleClaimTransformer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}" );
                 endpoints.MapRazorPages();
             } );
        }
    }
}
