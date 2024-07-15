using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UrlLoader.Database;
using UrlLoader.Producer.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������������
var configuration = builder.Configuration;

// ����������� �������� � DI-����������

// PostgreSQL connection
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));



// RabbitMQ connection
var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMQConnection");

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // ����� ������� ����������� ��������� ��� ����������� � RabbitMq
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddScoped<IFileEntityRepository, FileEntityRepository>();
// Swagger setup
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UrlLoader", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

// ��������� pipeline ��������� ��������
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
