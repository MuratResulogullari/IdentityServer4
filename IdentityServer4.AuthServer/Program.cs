using IdentityServer4.AuthServer;
using IdentityServer4.AuthServer.Models;
using IdentityServer4.AuthServer.Repository;
using IdentityServer4.AuthServer.Seeds;
using IdentityServer4.AuthServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:SqlConnection"].ToString();
builder.Services.AddDbContext<CustomDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
// Add services to the container.
builder.Services.AddControllersWithViews();


// verilerin kendi memorisinde saklaması
//builder.Services.AddIdentityServer()
//                .AddInMemoryApiResources(Config.GetApiResources())
//                .AddInMemoryApiScopes(Config.GetApiScopes())
//                .AddInMemoryClients(Config.GetClients())
//                .AddInMemoryIdentityResources(Config.GetIdentityResources())
//                //.AddTestUsers(Config.GetUsers().ToList())
//                .AddDeveloperSigningCredential()// Senin için public key ve private key üretir tempkey.jwk dosyasında tutar oluşturur
//                                                //.AddSigningCredential(); // Gerçek ortam için credentialları belirlernen yer
//                .AddProfileService<CustomUserProfileService>()
//                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>(); // Resource credentials validator custom yazdık


//verilerin dbde tutulması


var assemblyName=typeof(Program).Assembly.GetName().Name;
builder.Services.AddIdentityServer()
    .AddConfigurationStore(
        options => options.ConfigureDbContext =
        config => config.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(assemblyName))
    )
    .AddOperationalStore(
        options => options.ConfigureDbContext =
        config => config.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(assemblyName))
    ).AddDeveloperSigningCredential()// Senin için public key ve private key üretir tempkey.jwk dosyasında tutar oluşturur
                                                //.AddSigningCredential(); // Gerçek ortam için credentialları belirlernen yer
     .AddProfileService<CustomUserProfileService>()
     .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>(); // Resource credentials validator custom yazdık

builder.Services.AddScoped<ICustomUserRepository, CustomUserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
using var context = services.GetRequiredService<ConfigurationDbContext>();
    IdentityServerSeedData.Seed(context);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Identity Server 4 Middlewaresini eklemek zorundasi
app.UseIdentityServer();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();