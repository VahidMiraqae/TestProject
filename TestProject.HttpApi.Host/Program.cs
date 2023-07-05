using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using TestProject.Application;
using TestProject.Application.Dtos;
using TestProject.Core;
using TestProject.Ef;
using TestProject.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebDbContext>(options =>
    options.UseSqlite("Data Source=d:\\webDb.db;"));

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IMessageProducer, RabbitMqProducer>();
builder.Services.AddTransient<IOrderPolicy, OrderPolicy>();

builder.Services.AddHttpClient("warehouse", httpclient =>
{
    httpclient.BaseAddress = new Uri("https://localhost:7289");
});

var mapperConfig = new MapperConfiguration(config =>
{
    config.CreateMap<PlaceOrderDto, Order>();
    config.CreateMap<PlaceOrderItemDto, OrderItem>()
        .ForMember(a => a.Quantity, b => b.MapFrom(c => c.Count));
    config.CreateMap<Order, OrderDto>();
    config.CreateMap<OrderItem, OrderItemDto>();
    config.CreateMap<OrderItem, OrderedProductDto>()
        .ForMember(aa => aa.Id, b => b.MapFrom(c => c.ProductId));

});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
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
