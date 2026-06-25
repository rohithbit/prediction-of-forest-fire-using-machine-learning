ocelot.json
Sumit Mitra(IN8230)
25-06-2026 09:26
{
    "Routes": [
        {
        "DownstreamPathTemplate" : "/api/order",
        "DownstreamScheme" : "http",
        "DownstreamHostAndPorts" : [{
            "Host" : "localhost",
            "Port" : 8080
        }
    ],
    "UpstreamPathTemplate" : "/order-api/order",
    "UpstreamHttpMethod" : ["GET", "POST"]
    }
],
"GlobalConfiguration": {}
}
Sumit Mitra(IN8230)
25-06-2026 09:27
apigateway-program.cs
Sumit Mitra(IN8230)
25-06-2026 09:27
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
 
 
var builder = WebApplication.CreateBuilder(args);
 
// builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//     .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
//     .AddEnvironmentVariables();
 
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
 
builder.Services.AddControllers();
builder.Services.AddOcelot();
 
var app = builder.Build();
 
// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }
 
// app.UseHttpsRedirection();
// app.UseRouting();
// app.UseAuthorization();
 
// app.MapControllers();
await app.UseOcelot();
 
app.Run();
 
Sumit Mitra(IN8230)
25-06-2026 09:27
ordercontroller
Sumit Mitra(IN8230)
25-06-2026 09:27
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
 
namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly OrderDbContext db;
        public OrderController(OrderDbContext db1){
            db = db1;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(){
            return await db.Orders.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order){
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return StatusCode(201, new{
                message = "Order created successfully",
                order
            });
        }
    }
}
Sumit Mitra(IN8230)
25-06-2026 09:28
datafolder/appdbcontext
Sumit Mitra(IN8230)
25-06-2026 09:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;
using Microsoft.EntityFrameworkCore;
 
namespace OrderService.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options){
        }
        public DbSet<Order> Orders{get;set;}
    }
}
Sumit Mitra(IN8230)
25-06-2026 09:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace OrderService.Models
{
    public class Order
    {
        public int OrderId{get;set;}
        public string CustomerName{get;set;}
        public DateTime OrderDate {get;set;}
        public decimal TotalAmount {get;set;}
    }
}
Sumit Mitra(IN8230)
25-06-2026 09:28
program.cs order
Sumit Mitra(IN8230)
25-06-2026 09:28
using System.Buffers;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
var con = "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False";
builder.Services.AddControllers();
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(con));
 
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
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
