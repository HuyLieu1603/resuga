
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using PD_Store.Repositories.Auth;
using PD_Store.Repositories.Product;


namespace Dashboard.Extension
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            //services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IProductService, ProductService>();

        }
    
        //cookie
        public static void ConfigureCookie(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = $"/Home/NotAccess";
                options.Cookie.Name = ".AspNet.SharedCookie";
                options.Cookie.Path = "/";
                options.Cookie.Domain = configuration.GetValue<string>("CookieDomain");
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
            });


            //shared cookie
            var keysFolder = new DirectoryInfo(@"C:\OharaiWorkspaceSSO\keys");
            if (!keysFolder.Exists)
            {
                keysFolder.Create();
            }
            services.AddDataProtection()
                    .PersistKeysToFileSystem(keysFolder)
                    .SetApplicationName("NMViWorkspace");
        }

       
    }
}