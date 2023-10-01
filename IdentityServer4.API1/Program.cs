using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Kimlik doğrulama
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //Authentication Instance tutmak için custom shema tutatrsın iki türlü Üyelik sistemin varsa
                 .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.Authority = "https://localhost:7287";// Jwt token accesss token yayınlayan kim AuthServer endpoint veriyoruz.
                     // Yetkiliden public key alacak token içerisindeki private key ile doğrulama yapacak
                     options.Audience = "resource_api1"; // Bana bir token geldiğinde bu tokenda resource name olması lazım
                     // token içerisinde aud alanında bu isim olması lazım
                 });

// Yetkilendirme
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadProduct", policy => // Bir Policy şart koşmak
    {
        policy.RequireClaim("scope", "api1.read"); // Şartın gereksinimi ise claimType scope olacak tokendaki scope içinde api1.read olacak.
    });
    options.AddPolicy("CreateOrUpdateProduct", policy =>
    {
        policy.RequireClaim("scope", new[] { "api1.update", "api1.write" }); // burdada gereksinimine ikisinden en az birisi olması şart dedik.
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme, // Üyelik şeması  AddJwtBearer verilen ile aynı olması gerekiyorki aynı üyelik sistemini doğrulasın
        BearerFormat = "JWT" // Optional
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                          Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header
                        },
                         new List<string>()
                    }
                });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Doğrulamayı yapacak olan Middleware eklemeyi unutmuyoruz
app.UseAuthorization(); // Yetkilendirme yapapcak olan  Middleware

app.MapControllers();

app.Run();