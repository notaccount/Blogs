using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Power.Repository;
using Power.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Power.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Http;

namespace CommonPower.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            string ProviderName = Configuration.GetConnectionString("ProviderName");

            if (ProviderName == "SqlServer")
            {
                services.AddDbContext<DataContext>(options => options.UseSqlServer(ConnectionString, b => b.UseRowNumberForPaging()));
            }
            else if (ProviderName == "SqlLite")
            {
                services.AddDbContext<DataContext>(options => options.UseSqlite(ConnectionString));
            }

            services.AddMemoryCache();

            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(100 * 60);
            });

            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenInfo.WebCookieInstance;
                options.DefaultSignInScheme = CookieAuthenInfo.WebCookieInstance;
                options.DefaultAuthenticateScheme = CookieAuthenInfo.WebCookieInstance;
            }).AddCookie(CookieAuthenInfo.WebCookieInstance, m =>
                    {
                        m.LoginPath = new PathString("/Login/Index");
                        m.LogoutPath = new PathString("/Login/Logout");
                        m.Cookie.Path = "/";
                    });
            services.AddMvc(options => options.MaxModelValidationErrors = 50);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            //InitializeNetCoreBBSDatabase(app.ApplicationServices);
            app.UseSession();
            app.UseAuthentication();
            //app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                   name: "blogs",
                   template: "Index",
                   defaults: new { controller = "Blogs", action = "Index" });


                routes.MapRoute(
                    name: "default",
                    template: "Blogs/Index",
                    defaults: new { controller = "Blogs", action = "Index" });
            });

            InitializeNetCoreBBSDatabase(app.ApplicationServices);
        }

        private void InitializeNetCoreBBSDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<DataContext>();

                if (db.Database != null && db.Database.EnsureCreated())
                {
                    db.PowerUser.AddRange(CreateUserList());
                    db.SaveChanges();
                }
                else
                {
                    //db.Database.Migrate();
                }
            }
        }


        public List<PowerUser> CreateUserList()
        {
            List<PowerUser> list = new List<PowerUser>();
            list.Add(new PowerUser() { Id = Guid.NewGuid(), UID = "abc" });
            return list;
        }
    }

    public class CookieAuthenInfo
    {
        public static string WebCookieInstance = "MyInstance";
    }
}
