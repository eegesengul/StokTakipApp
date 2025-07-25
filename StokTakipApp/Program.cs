using System.Globalization;
using Microsoft.OpenApi.Models;
using SD.LLBLGen.Pro.DQE.PostgreSql;
using SD.LLBLGen.Pro.ORMSupportClasses;
using stoktakip.DatabaseSpecific;

var builder = WebApplication.CreateBuilder(args);

// LLBLGen ayarlarý
RuntimeConfiguration.ConfigureDQE<PostgreSqlDQEConfiguration>(c =>
    c.AddDbProviderFactory(typeof(Npgsql.NpgsqlFactory))
);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StokTakipApp API", Version = "v1" });
});

var app = builder.Build();

// Kültür ayarý
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StokTakipApp API v1");
});

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// API Controllerlar ve MVC Controllerlar için gerekli!
app.MapControllers(); // API Controllerlar için

// Razor View'lar için klasik route (Products, Home vs.)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();