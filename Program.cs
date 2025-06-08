using Dashboard.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PD_Store.DbContextFolder;
using PD_Store.Models.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AdminDbContext>(
              options => options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection"))
            );

builder.Services.AddCustomServices();

// builder.Services.ConfigureCookie(builder.Configuration);
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".Resuga.Auth";
    // options.Cookie.SameSite = SameSiteMode.None;                 // bắt buộc phải là None nếu cross-origin
    // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;    // bắt buộc HTTPS

    options.Cookie.SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.None;
    options.Cookie.SecurePolicy = isDevelopment ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
});

//Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
    opt.Password.RequiredLength = 1;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.User.RequireUniqueEmail = false;
    //opt.User.Emai
})
 .AddEntityFrameworkStores<AdminDbContext>()
 .AddDefaultTokenProviders();

// Read Kestrel configuration
builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
