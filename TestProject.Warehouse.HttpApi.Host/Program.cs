using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TestProject.Application.Dtos;
using TestProject.Core;
using TestProject.Ef;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseSqlite("Data Source=d:\\warehouseDb.db;"));

var mapperConfig = new MapperConfiguration(config =>
{
    config.CreateMap<Product, ProductDto>(); 
    config.CreateMap<Product, ProductWithQuantityDto>();

});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);



var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: "orders",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);



var app = builder.Build();

// rabbit
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var orderedProductsDic = JsonConvert.DeserializeObject<OrderedProductDto[]>(message)
    .ToDictionary(aa => aa.Id, aa => aa);

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var products = dbContext.GetProductList(orderedProductsDic.Values.Select(aa => aa.Id)).Result;
        foreach (var product in products)
        {
            product.Quantity -= orderedProductsDic[product.Id].Quantity;
        }
        dbContext.UpdateProducts(products);
    } 
};
channel.BasicConsume(queue: "orders",
                     autoAck: true,
                     consumer: consumer);
// rabbit end

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}


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
