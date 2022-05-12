using AutoMapper;
using B.AdvertisementApp.Business.DependencyResolvers.Microsoft;
using B.AdvertisementApp.Business.Helpers;
using B.AdvertisementApp.UI.Mappings.Automapper;
using B.AdvertisementApp.UI.Models;
using B.AdvertisementApp.UI.ValidationForModels;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddTransient<IValidator<UserCreateModel>, UserCreateModelValidator>();

var profiles = ProfileHelper.GetProfiles();
profiles.Add(new UserCreateModelProfile());
var configuration = new MapperConfiguration(opt =>
  {
      opt.AddProfiles(profiles);
  });
var mapper= configuration.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
