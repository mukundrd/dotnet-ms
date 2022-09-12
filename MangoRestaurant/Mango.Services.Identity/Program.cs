using Mango.Services.Identity;
using Mango.Services.Identity.Initializer;
using Mango.Services.Identity.Models;
using Mango.Services.ProductsAPI.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

var identityBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(SD.IdentityResources)
    .AddInMemoryApiScopes(SD.ApiScopes)
    .AddInMemoryClients(SD.Clients)
    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

identityBuilder.AddDeveloperSigningCredential();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IDbInitializer>().Initialize();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
