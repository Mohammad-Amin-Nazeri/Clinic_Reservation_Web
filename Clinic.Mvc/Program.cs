using System.Text.Encodings.Web;
using System.Text.Unicode;
using Clinic.Application.Services.Implementation;
using Clinic.Application.Services.Interfaces;
using Clinic.Data.Context;
using Clinic.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>));
builder.Services.AddScoped<IOtpService , OtpService>();
builder.Services.AddScoped<ISmsService , SmsService>();
builder.Services.AddScoped<IRecordService , RecordService>();
builder.Services.AddScoped<IReservationService , ReservationService>();
builder.Services.AddScoped<IUserService , UserService>();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/log-out";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

// Encode HTML notifications text
builder.Services.AddSingleton((HtmlEncoder.Create(allowedRanges: [UnicodeRanges.BasicLatin , UnicodeRanges.Arabic])));

#region Data Protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\AuthKeys"))
    .SetApplicationName("Clinic")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(7));
#endregion

var app = builder.Build();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Areas Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
