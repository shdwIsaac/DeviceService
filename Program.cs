using Confluent.Kafka;
using DeviceService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/devices/{id:long}", (long id) => new DeviceResponse
    {
        id = id,
        status = "on",
        created_at = DateTime.Now,
        house_id = 1,
        serial_number = "A123",
        type = "Telemetry"
    })
    .WithOpenApi();

app.MapPut("/devices/{id:long}/status", (long id, [FromBody]StatusRequest request) => new StatusResponse
    {
        id = id,
        status = request.Status,
        updated_at = DateTime.Now
    })
    .WithOpenApi();

app.MapPost("/devices/{id:long}/commands", (long id, [FromBody]CommandRequest request) =>
    {
        var config = new ProducerConfig {BootstrapServers = "localhost:9092"};
        using var producer = new ProducerBuilder<string, string>(config).Build();
        var result = producer.ProduceAsync("telemetry-data", new Message<string, string> { Value = "Device" });
    })
    .WithOpenApi();

app.Run();