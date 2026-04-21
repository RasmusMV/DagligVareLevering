using DagligVareLevering.EFDbContext;
using Microsoft.EntityFrameworkCore;
using DagligVareLevering.Service;
using DagligVareLevering.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IService<BasketItem>, DbGenericService<BasketItem>>();
builder.Services.AddScoped<IService<Order>, DbGenericService<Order>>();
builder.Services.AddScoped<IService<OrderLine>, DbGenericService<OrderLine>>();
builder.Services.AddScoped<IService<Product>, DbGenericService<Product>>();
builder.Services.AddScoped<IService<Store>, DbGenericService<Store>>();
builder.Services.AddScoped<IService<User>, DbGenericService<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
