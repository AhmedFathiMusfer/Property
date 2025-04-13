using Microsoft.AspNetCore.Authentication.Cookies;
using Property_Wep.Extensions;
using Property_Wep.MapppingConfig;
using Property_Wep.Services;
using Property_Wep.Services.IServices;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews(u=>u.Filters.Add(new AuhExceptionRedirection()));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;  
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(option =>
   {
       option.Cookie.HttpOnly = true;
       option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
       option.SlidingExpiration = true;
       option.LoginPath = "/Auth/Login";
       option.AccessDeniedPath = "/Auth/AccessDenied";
   });
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddHttpClient<IVillaService, VillaService>();
builder.Services.AddScoped<IVillaService, VillaService>();

builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IApiRequsetMessageBuilder , ApiRequsetMessageBuilder>();

builder.Services.AddScoped<ITokenProvider, TokenProvider>();    


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

app.UseAuthorization(); 
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
