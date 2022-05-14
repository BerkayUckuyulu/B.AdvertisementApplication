using AutoMapper;
using B.AdvertisementApp.Business.DependencyResolvers.Microsoft;
using B.AdvertisementApp.Business.Helpers;
using B.AdvertisementApp.UI.Mappings.Automapper;
using B.AdvertisementApp.UI.Models;
using B.AdvertisementApp.UI.ValidationForModels;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "B.Advertisement";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromDays(20);
        options.LoginPath = new PathString("/Account/SigIn");
        options.LogoutPath = new PathString("/Account/LogOut");
        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
        
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
