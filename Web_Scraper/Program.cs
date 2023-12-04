using Microsoft.EntityFrameworkCore;
using Web_Scraper;
using Web_Scraper.Clients;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
Scraper scraper = new Scraper();
//scraper.GetPhoneInfo();
var serviceProvider = builder.Services.BuildServiceProvider();
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    var data = scraper.GetPhoneInfo();
    var phones = data.Phone;
    var stores = data.Store;
    dbContext.PhoneInfo.AddRange(phones);
    dbContext.StoreInfo.AddRange(stores);
    dbContext.SaveChanges();
}


app.Run();
