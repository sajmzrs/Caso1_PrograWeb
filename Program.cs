using Caso1.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ReservaDatos>();
var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
app.UseRequestLocalization("en-US");
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();

