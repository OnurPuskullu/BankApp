using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;
using Udemy.BankApp.Web.Data.UnitOfWork;
using Udemy.BankApp.Web.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BankContext>(opt =>
{
    opt.UseSqlServer("server=LAPTOP-HUG0NL9Q;database=BankDb;uid=sa;pwd=Test123!;TrustServerCertificate=True;");
});
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositories<>));
//builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
//builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUow, Uow>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IAccountMapper,AccountMapper>();
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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine
    (Directory.GetCurrentDirectory(), "node_modules")),
    RequestPath = "/node_modules"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
