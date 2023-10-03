using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Resource owner credential flow için bunlar yeterli çünkü kendi login sayfasını kullanacak openId kullanmıyacak
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "Cookies";
//}).AddCookie("Cookies", options =>
//{
//    options.LoginPath = "/Login/Login";
//    options.LogoutPath = "/Login/logout";
//});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//"Cookies"; sabitleri kullanmak dahah mantıklı hata yapmamak için yada kendi sabitlerini oluştur
    options.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", options =>
{
    options.AccessDeniedPath = "/Home/AccessDenied";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:7287";
    options.ClientId = "WebAppMVC1";
    options.ClientSecret = "secret";
    options.ResponseType = "code id_token"; // code token  , code id_token , code id_token token  response typelar 
    options.GetClaimsFromUserInfoEndpoint = true;// Arka plande userinfo endpoint istek atıp user infoları claims içerisinde ekler
    options.SaveTokens = true;// Başaşrılı bir Authentication sağlandıktan sonra Authorization için token kaydet true ise 
    options.Scope.Add("api1.read"); // Eğerki bu istek clientin AllowedScopes larında taımlanmışsa hata verecektir
    options.Scope.Add("offline_access");
    options.Scope.Add("CountryAndCity");
    options.Scope.Add("Roles");
    options.ClaimActions.MapUniqueJsonKey("country", "country");// profiledan buraya map  yaptık
    options.ClaimActions.MapUniqueJsonKey("city", "city");
    options.ClaimActions.MapUniqueJsonKey("role", "role");
    // Role için yapılması için 
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        RoleClaimType = "role"
    };
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
