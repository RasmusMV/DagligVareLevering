using DagligVareLevering.EFDbContext;
using Microsoft.EntityFrameworkCore;
using DagligVareLevering.Service;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories;
using DagligVareLevering.Service.Interfaces;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Observers.Interfaces;
using DagligVareLevering.Observers;
using DagligVareLevering.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
//Repos
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(GenericService<>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketItemService, BasketItemService>();
//Observers
builder.Services.AddScoped<IOrderObserver, OrderObserver>();
//Handlers 
builder.Services.AddSingleton<OrderEventsHandler>();

var app = builder.Build();

app.Services.GetRequiredService<OrderEventsHandler>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
