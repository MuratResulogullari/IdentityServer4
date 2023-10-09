using IdentityServer4.Weather.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Kimlik doğrulama
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //Authentication Instance tutmak için custom shema tutatrsın iki türlü Üyelik sistemin varsa
                 .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.Authority = builder.Configuration["IdentityApiConfiguration:Authority"];// Jwt token accesss token yayınlayan kim AuthServer endpoint veriyoruz.
                     // Yetkiliden public key alacak token içerisindeki private key ile doğrulama yapacak
                     options.Audience = builder.Configuration["IdentityApiConfiguration:Audience"]; // Bana bir token geldiğinde bu tokenda resource name olması lazım
                                                                                                    // token içerisinde aud alanında bu isim olması lazım
                     options.RequireHttpsMetadata = false;// change builder.Environment.IsProduction();
                     options.SaveToken = true;
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateLifetime = true,
                     };
                 });

// Yetkilendirme
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ResourceOwnerPolicy", policy => // Bir Policy şart koşmak
//    {
//        policy.RequireClaim("scope", "IdentityServerApi", "WeatherApi"); // Şartın gereksinimi ise claimType scope olacak tokendaki scope içinde api1.read olacak.
//    });
//    options.AddPolicy("CreateOrUpdateProduct", policy =>
//    {
//        policy.RequireClaim("scope", new[] { " weatherforecast.modify", "weatherforecast.write" }); // burdada gereksinimine ikisinden en az birisi olması şart dedik.
//    });
//});
builder.Services.AddHttpClient("IdentityServer", opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["IdentityApiConfiguration:Authority"]);
});
builder.Services.AddScoped<IAuthorizationHandler, ProtectedResourcePermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ResourceOwnerPolicy", policyBuilder => policyBuilder.AddRequirements(new ProtectedResourceAuthorizationRequirement(serviceName: builder.Configuration["IdentityApiConfiguration:Audience"])));
});


string[] allowedApiScopes = new string[] {"IdentityServerApi", "WeatherApi" };
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {  Title = "Weather Forecast API", Version = "v1" });
    var openApiSecurityScheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{builder.Configuration["IdentityApiConfiguration:Authority"]}/connect/authorize"),
                TokenUrl = new Uri($"{builder.Configuration["IdentityApiConfiguration:Authority"]}/connect/token"),
                //Scopes = new Dictionary<string, string> { ["weather_api_scope"] = "weather_api_scope"  }
                Scopes = new Dictionary<string, string>()
            }
        }
    };
    foreach (var scope in allowedApiScopes)
    {
        openApiSecurityScheme.Flows.AuthorizationCode.Scopes.Add(new KeyValuePair<string, string>(scope, scope));
    };

    options.AddSecurityDefinition("oauth2", openApiSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                }
                            },
                            allowedApiScopes
                    }
                            });

    options.OperationFilter<AuthorizeOperationFilter>();
});

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger()
     .UseSwaggerUI(options =>
     {
         options.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API");
         options.OAuthClientId("weather_api_swagger");
         options.OAuthAppName("Weather API");
         options.OAuthUsePkce();
     });
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();