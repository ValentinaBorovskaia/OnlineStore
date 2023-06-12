using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Interfaces;
using CatalogService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
builder.Services.AddSwaggerGen(options=>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
            }
            });
}
);


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

var jwtSecretKey = configuration["IdentityServerSettings:JwtSecretKey"];
var jwtIssuer = configuration["IdentityServerSettings:JwtIssuer"];
var jwtAudience = configuration["IdentityServerSettings:JwtAudience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

