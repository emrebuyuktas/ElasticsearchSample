using ElasticsearchSample.API.Extensions;
using ElasticsearchSample.API.Repositories;
using ElasticsearchSample.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region ServiceRegistration
builder.Services.AddScoped<ProductService>();
builder.Services.AddElasticsearch(builder.Configuration);
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ECommerceRepository>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<BlogRepository>();
#endregion

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
