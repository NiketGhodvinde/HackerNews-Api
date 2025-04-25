using HackerNewsApi.Config;
using HackerNewsApi.Interface;
using HackerNewsApi.Repositories;
using HackerNewsApi.Service;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<Settings>(
    builder.Configuration.GetSection("HackerNewsApi"));

builder.Services.AddHttpClient<IStoryRepository, StoryRepository>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<Settings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
});

// Register other services
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddMemoryCache();
builder.Services.AddLogging();

builder.Services.AddControllers();

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

app.UseCors("AllowAngularDevClient");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
