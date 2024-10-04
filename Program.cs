using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using DeviceService;
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

app.MapGet("/devices/{id:long}", async (long id) =>
    {
        using HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://smart-home-monolith:8080");
        using HttpResponseMessage response = await httpClient.GetAsync($"/api/heating/{id}");
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResponse>(jsonResponse);
    })
    .WithOpenApi();


app.MapPut("/devices/{id:long}/status", async (long id, [FromBody] StatusRequest request) =>  {
        using HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://smart-home-monolith:8080");
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await httpClient.PutAsync($"/api/heating/{id}", content);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResponse>(jsonResponse);
    })
    .WithOpenApi();

app.MapPost("/devices/{id:long}/commands", async (long id, [FromBody] CommandRequest request) =>
    {
        using HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://smart-home-monolith:8080");
        using HttpResponseMessage response = await httpClient.GetAsync($"/api/heating/{id}/{request.Command}");
        response.EnsureSuccessStatusCode();
    })
    .WithOpenApi();

app.Run();