using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json;
using Microsoft.FeatureManagement.FeatureFilters;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()   
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.IgnoreNullValues = true;
                });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions (apiDescriptions => apiDescriptions.First ());
    }
);

//adding feature management
builder.Services
    .AddFeatureManagement()
    .AddFeatureFilter<PercentageFilter>();
    
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
