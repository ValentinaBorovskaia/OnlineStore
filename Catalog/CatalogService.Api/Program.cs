using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Interfaces;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

var connectionString = configuration.GetConnectionString("CatalogDb");
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IItemRepository, ItemRepository>();

builder.Services.AddTransient<IItemService, ItemService>();

builder.Services.AddSingleton<IMessageProducer>(x =>
    new MessageProducer(
        new RabbitMQ.Client.ConnectionFactory()
        {
            HostName = builder.Configuration["RabbitMq:HostName"],
         //   UserName = builder.Configuration["RabbitMq:UserName"],
         //   Password = builder.Configuration["RabbitMq:Password"]
        }
    )
);

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "catalogApi";
        options.Authority = "https://localhost:44322";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

