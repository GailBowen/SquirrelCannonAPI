using Microsoft.EntityFrameworkCore;
using SquirrelCannon.Data;

var builder = WebApplication.CreateBuilder(args);

// Change this line from AddRazorPages to AddControllersWithViews
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<FlashcardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Remove these lines as they're Razor Pages specific
// app.MapStaticAssets();
// app.MapRazorPages()
//    .WithStaticAssets();

// Update the default route to use Home controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
