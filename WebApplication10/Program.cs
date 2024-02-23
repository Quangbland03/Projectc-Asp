﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication10.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebApplication10Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplication10Context") ?? throw new InvalidOperationException("Connection string 'WebApplication10Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
//Set Session Timeout. Default is 20 minutes.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
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

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
