using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Platform.Application.Schemas;
using Platform.Core.Schemas;
using Platform.Infrastructure.Schemas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()      // разрешаем любые домены
            .AllowAnyMethod()      // разрешаем любые HTTP-методы (GET, POST, PUT, DELETE,...)
            .AllowAnyHeader();     // разрешаем любые заголовки
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IJsonSchemaRepository, FileJsonSchemaRepository>();
builder.Services.AddScoped<ISchemaService, SchemaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();