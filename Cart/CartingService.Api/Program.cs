using CartingService.Api.Option;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using CartingService.DAL.Interface;
using CartingService.BLL.Interfaces;
using CartingService.BLL.Services;
using CartingService.DAL.Repositories;
using CartingService.Api.BackgroundServices;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using CartingService;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IdentityServerSettings>(builder.Configuration.GetSection("IdentityServerSettings"));
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "cartApi";
        options.Authority = "https://localhost:5000";
    });


builder.Services.AddScoped<ILiteDatabase>(x => new LiteDatabase(builder.Configuration.GetConnectionString("LiteDb")));
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddHostedService<ItemChangesQueueBackgroundService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.ApiVersion.ToString()
            );

        options.OAuthClientId("demo_api_swagger");
        options.OAuthAppName("Demo API - Swagger");
        options.OAuthUsePkce();
    }
    
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                           context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        if (hasAuthorize)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}}]
                            = new[] {"api1"}
                    }
                };
        }
    }
}