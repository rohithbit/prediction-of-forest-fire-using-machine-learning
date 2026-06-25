ProductController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Data;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        public ApplicationDbContext db;
        public ProductController(ApplicationDbContext db)
        {
            this.db=db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts(){
            var prods=db.Products.ToList();
            return Ok(prods);
        }
        [HttpGet("{id}")]
        public ActionResult<Product>GetProductById(int id){
        Product pro=db.Products.FirstOrDefault(p=>p.Id==id);
        if(pro==null)return NotFound();
        return Ok(pro);
        }
        [HttpGet("filter")]
        public ActionResult<IEnumerable<Product>>GetProductsByCategory([FromQuery] string category){
            List<Product>prods=db.Products.Where(p=>p.Category==category).ToList();
            if(prods==null){
                return NotFound();
            }
            return Ok(prods);
        }
        [HttpPost]
        public ActionResult<Product>CreateProduct(Product product){
             
             db.Products.Add(product);
             db.SaveChanges();
            return CreatedAtAction("CreateProduct",product);
 
        }
    }
}
 
Data/ApplicationDbContext.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
namespace dotnetapp.Data
{
    public class ApplicationDbContext :DbContext
    {
       
        public DbSet<Product>Products{get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options){
           
        }
    }
}
 
Models/Product.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
    public class Product
    {
        [Key]
        public int Id{get;set;}
        public string Name{get;set;}
        public string Category{get;set;}
        public decimal Price{get;set;}
        public int Stock{get;set;}
 
    }
}
 
Program.cs
using dotnetapp.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
var conn="User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at Get started with Swashbuckle and ASP.NET Core | Microsoft Learn
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(app=>app.UseSqlServer(conn));
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
 
Get started with Swashbuckle and ASP.NET Core | Microsoft Learn
Learn how to add Swashbuckle to your ASP.NET Core web API project to integrate the Swagger UI.
 
2nd-Controllers/ArtWorkController.cs
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/artworks")]
    public class ArtworkController : ControllerBase
    {
   public ApplicationDbContext db;
      public ArtworkController(ApplicationDbContext db)
      { this.db=db;
       
      }
      [HttpGet]
      public ActionResult<IEnumerable<Artwork>>GetArtworks(){
        return Ok(db.Artworks.ToList());
      }
      [HttpGet("{id}")]
      public ActionResult<Artwork>GetArtworkById(int id){
        Artwork work=db.Artworks.FirstOrDefault(a=>a.ArtworkId==id);
        if(work==null){
          return NotFound();
        }
        return Ok(work);
      }
      [HttpGet("filter")]
      public ActionResult<IEnumerable<Artwork>>GetArtworksByArtist([FromQuery] string artist)
      {
           List<Artwork> work=db.Artworks.Where(a=>a.Artist==artist).ToList();
        if(work==null){
          return NotFound();
        }
        return Ok(work);
      }
      [HttpPost]
      public ActionResult<Artwork>CreateArtwork(Artwork work){
        db.Artworks.Add(work);
        db.SaveChanges();
        return CreatedAtAction("CreateArtwork",work);
      }
    }
}
 
Models/ApplicationDbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace dotnetapp.Models
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Artwork>Artworks{get;set;}
 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options){
 
        }
       
    }
}
 
Models/Artwork.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class Artwork
    {
        public int ArtworkId{get;set;}
        public string Title{get;set;}
        public string Artist{get;set;}
        public int Year{get;set;}
        public string Medium{get;set;}
        public string Description{get;set;}
    }
}
 
Program.cs
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
var conn="User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at Get started with Swashbuckle and ASP.NET Core | Microsoft Learn
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(db=>db.UseSqlServer(conn));
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
 
get; set
Get started with Swashbuckle and ASP.NET Core
Learn how to add Swashbuckle to your ASP.NET Core web API project to integrate the Swagger UI.
