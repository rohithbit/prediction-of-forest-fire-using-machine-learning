MobilePhoneControllers
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobilePhoneController : Controller
    {
        public IMobilePhoneService service;
       public MobilePhoneController(IMobilePhoneService service)
       {
        this.service=service;
       }
     
       [HttpGet]
       public IActionResult GetAllPhones(){
        return Ok(service.GetMobilePhones());
       }
       [HttpGet("{id}")]
       public IActionResult GetMobilePhoneById(int id){
        if(service.GetMobilePhone(id)==null){
   return NotFound();
        }
        return Ok(service.GetMobilePhone(id));
       }
       [HttpPost]
       public IActionResult AddMobilePhone(MobilePhone mobilePhone){
        MobilePhone mb=service.SaveMobilePhone(mobilePhone);
        return CreatedAtAction("AddMobilePhone",mobilePhone);
       }
       [HttpPut("{id}")]
       public IActionResult UpdateMobilePhone(int id,MobilePhone mobilePhone){
         if(service.GetMobilePhone(id)==null){
   return NotFound();
        }
        service.UpdateMobilePhone(id,mobilePhone);
        return NoContent();
       }
       [HttpDelete("{id}")]
       public IActionResult DeleteMobilePhone(int id){
          if(service.GetMobilePhone(id)==null){
   return NotFound();
        }
        if(service.DeleteMobilePhone(id)){
            return NoContent();
        }
        return NotFound();
       }
    }
}
 
IMobileServices
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
 
namespace dotnetapp.Services
{
    public interface IMobilePhoneService
    {
        List<MobilePhone> GetMobilePhones();
        MobilePhone GetMobilePhone(int id);
        MobilePhone SaveMobilePhone(MobilePhone mobilePhone);
        MobilePhone UpdateMobilePhone(int id,MobilePhone mobilePhone);
        bool DeleteMobilePhone(int id);
    }
}
 
MobilePhoneServices
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Repository;
 
namespace dotnetapp.Services
{
    public class MobilePhoneService:IMobilePhoneService
    {
        public MobilePhoneRepository repository;
        public MobilePhoneService(MobilePhoneRepository repository)
        {
            this.repository=repository;
           
        }
        public MobilePhoneService(){
           
        }
        public List<MobilePhone> GetMobilePhones(){
         return repository.GetMobilePhones();
        }
        public MobilePhone GetMobilePhone(int id){
            return repository.GetMobilePhone(id);
        }
        public MobilePhone SaveMobilePhone(MobilePhone mobilePhone){
            return repository.SaveMobilePhone(mobilePhone);
        }
        public MobilePhone UpdateMobilePhone(int id,MobilePhone mobilePhone){
            return repository.UpdateMobilePhone(id,mobilePhone);
        }
        public bool DeleteMobilePhone(int id){
            return repository.DeleteMobilePhone(id);
        }
       
    }
}
 
MobilePhone model
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class MobilePhone
    {
        public int MobilePhoneId{get;set;}
        public string Brand{get;set;}
        public string Model{get;set;}
        public decimal Price{get;set;}
        public int StockQuantity{get;set;}
    }
}
 
MobilePhoneRepository
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
 
namespace dotnetapp.Repository
{
    public class MobilePhoneRepository
    {
        public MobilePhoneRepository()
        {
           
        }
        public static List<MobilePhone>mobiles{get;set;}=new List<MobilePhone>();
        public MobilePhone SaveMobilePhone(MobilePhone mobilePhone){
          mobilePhone.MobilePhoneId=mobiles.Count()+1;
          mobiles.Add(mobilePhone);
          return mobilePhone;
        }
        public List<MobilePhone> GetMobilePhones(){
            return mobiles;
        }
        public MobilePhone UpdateMobilePhone(int id,MobilePhone mobilePhone){
            MobilePhone ph=mobiles.FirstOrDefault(m=>m.MobilePhoneId==id);
            ph=mobilePhone;
            return ph;
        }
        public bool DeleteMobilePhone(int id){
                        MobilePhone ph=mobiles.FirstOrDefault(m=>m.MobilePhoneId==id);
                        mobiles.Remove(ph);
                        return true;
                   
        }
        public MobilePhone GetMobilePhone(int id){
              return mobiles.FirstOrDefault(m=>m.MobilePhoneId==id);
        }
    }
}
 
Program.cs
 
using dotnetapp.Repository;
using dotnetapp.Services;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add Event services to the container.
 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMobilePhoneService,MobilePhoneService>();
builder.Services.AddScoped<MobilePhoneRepository>();
var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
 
