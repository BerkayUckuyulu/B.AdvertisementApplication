using B.AdvertisementApp.Business.DependencyResolvers.Microsoft;
using B.AdvertisementApp.UI.Models;
using B.AdvertisementApp.UI.ValidationForModels;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddTransient<IValidator<UserCreateModel>, UserCreateModelValidator>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
