var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", options =>
{
    options.LoginPath = "/accunt/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/accessdenied";
}).AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:7287";
    options.ClientId= "WebAppMVC1";
    options.ClientSecret = "secret";
    options.ResponseType = "code id_token"; // code token  , code id_token , code id_token token  response typelar 
    options.GetClaimsFromUserInfoEndpoint = true;// Arka plande userinfo endpoint istek atıp user infoları claims içerisinde ekler
    options.SaveTokens=true;// Başaşrılı bir Authentication sağlandıktan sonra Authorization için token kaydet true ise 
    options.Scope.Add("api1.read"); // Eğerki bu istek clientin AllowedScopes larında taımlanmışsa hata verecektir
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
