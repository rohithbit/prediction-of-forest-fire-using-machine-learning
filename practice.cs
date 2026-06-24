using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace dotnetapp.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register(){
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model){
           return View();
        }
        [HttpGet]
        public IActionResult Login(){
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model){
            return View();
        }
        public IActionResult Logout(){
            return View();
        }
       
    }
}
 
=================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
namespace dotnetapp.Controllers
{
    [Route("books")]
    public class BookController : Controller
    {
        private static List<Book>books{get;set;}=new List<Book>();
        public ApplicationDbContext db;
     public BookController(ApplicationDbContext db){
this.db=db;
     }
 
     public IActionResult Index(){
         
        return View(books);
     }
     [HttpGet]
     public IActionResult Create(){
        return View();
     }
     [HttpPost]
     public IActionResult Create(Book book){
        book.BookId=books.Count+1;
        books.Add(book);
        return RedirectToAction("Index");
     }
   
     [HttpGet]
       [Route("")]
     public IActionResult IndexDbContext(){
        // var books=db.Books.ToList();
               ViewBag.vari=db.Books.ToList();
        return View();
     }
     [HttpGet]
     [Route("create")]
     public IActionResult CreateDbContext(){
   ViewBag.Title="";
        ViewBag.Author="";
        ViewBag.Category="";
        ViewBag.Price=0.0;
        ViewBag.LibraryId=0;
        return View();
     }
     [HttpPost]
     [Route("create")]
     public IActionResult CreateDbContext(Book book){
     
       
        db.Books.Add(book);
        db.SaveChanges();
 
        return View();
     }
     [HttpGet]
     public IActionResult EditDbContext(int id){
      var book=db.Books.FirstOrDefault(b=>b.BookId==id);
      if(book==null){
         return NotFound();
      }
      return View(book);
     }
      [HttpGet]
     public IActionResult EditDbContext(int id,Book book){
      var books=db.Books.FirstOrDefault(b=>b.BookId==id);
      if(books==null){
         return NotFound();
      }
      books=book;
      db.SaveChanges();
      return View(books);
     }
     [HttpGet]
     public IActionResult DeleteDbContext(int id){
        var books=db.Books.FirstOrDefault(b=>b.BookId==id);
      if(books==null){
         return NotFound();
      }
 
      return View(books);
     }
     [HttpPost]
 
   public IActionResult DeleteConfirmedDbContext(int id){
        var books=db.Books.FirstOrDefault(b=>b.BookId==id);
      if(books==null){
         return NotFound();
      }
      db.Books.Remove(books);
      db.SaveChanges();
 
      return RedirectToAction("Index");
     }
    }
}
=======================
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
 
namespace dotnetapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
 
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
 
        public IActionResult Index()
        {
            return View();
        }
 
        public IActionResult Privacy()
        {
            return View();
        }
 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
++======================
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
namespace dotnetapp.Controllers
{
    [Route("libraries")]
    public class LibraryController : Controller
    {
        public ApplicationDbContext db;
        public LibraryController(ApplicationDbContext db){
   this.db=db;
        }
        [Route("")]
         public IActionResult Index(){
   var libs=db.Libraries.ToList();
            return View(libs);
         }
         [HttpGet]
         [Route("create")]
         public IActionResult Create(){
            return View();
         }
         [HttpPost]
         [Route("create")]
         public IActionResult Create(Library library){
            db.Libraries.Add(library);
            db.SaveChanges();
            return View();
         }
    }
}
 
namespace dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
public class ApplicationDbContext :IdentityDbContext{
 
public DbSet<Book>Books{get;set;}
public DbSet<Library>Libraries{get;set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options){
 
    }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Library>()
    //     .HasMany(l=>l.Books)
    //     .WithOne(b=>b.Library)
    //     .HasForeignKey(b=>b.LibraryId);
    // }
 
}
========================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
    public class Book
    {
        [Key]
        public int BookId{get;set;}
        [Required(ErrorMessage ="Title is required.")]
        public string Title{get;set;}
         [Required(ErrorMessage ="Author is required.")]
        public string Author{get;set;}
         [Required(ErrorMessage ="Category is required.")]
        public string Category{get;set;}
         [Required(ErrorMessage ="Price amount is required.")]
         [Range(1.00,double.MaxValue,ErrorMessage ="Price amount must be greater than 0.")]
        public decimal Price{get;set;}
 
        public int LibraryId{get;set;}
        public Library Library{get;set;}
 
    }
}
+++++++++++++++++++++++++++++
 
namespace dotnetapp.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
 
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
+++++++++++++++++++++++++++++++++++
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
    public class Library
    {
        [Key]
        public int LibraryId{get;set;}
        [Required(ErrorMessage ="Name is required.")]
        public string Name{get;set;}
                [Required(ErrorMessage ="Address is required.")]
        public string Address{get;set;}
        [Range(0,int.MaxValue,ErrorMessage ="MaximumCapacity must be greater than or equal to 0.")]
        public int MaximumCapacity{get;set;}
        public ICollection<Book>Books{get;set;}
    }
}
=======================
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email{get;set;}
        [DataType(DataType.Password)]
        public string Password{get;set;}
    }
}
=========================
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
 
namespace dotnetapp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email{get;set;}
        [DataType(DataType.Password)]
        public string Password{get;set;}
           [DataType(DataType.Password)]
        public string ConfirmPassword{get;set;}
    }
}
 
 
 
Account-Login
@model dotnetapp.Models.LoginViewModel;
 
<h2>Login</h2>
<form asp-action="Login" asp-asp-controller="Account" method="post">
    <label asp-for="Email">Email</label>
    <input  asp-for="Email"/>
       <label asp-for="Password">Password</label>
    <input  asp-for="Password"/>
   
    <button type="submit">Login</button>
</form>
<p>Dont have an account? <a asp-action="Register">Register</a></p>
 
==========================
Account-register
<H2 id="Register">Register</H2>
===============
@model Book
<h1>Create Book</h1>
<form asp-action="Create" method="post">
    <label asp-for="Title">Title</label>
    <input asp-for="Title"/>
       <label asp-for="Author">Author</label>
    <input asp-for="Author"/>
       <label asp-for="Category">Category</label>
    <input asp-for="Category"/>
     <label asp-for="Price">Price</label>
    <input asp-for="Price"/>
    <button type="submit">Create</button>
</form>
 
=========================
 
<h1>Create Book</h1>
<form asp-action="CreateDbContext" method="post">
    <label >Title</label>
    <input type="text" name="Title"value="@ViewBag.Title"/>
       <label >Author</label>
      <input type="text" name="Author" value="@ViewBag.Author"/>
       <label >Category</label>
   <input type="text" name="Category" value="@ViewBag.Category"/>
     <label >Price</label>
    <input type="number" name="Price" value="@ViewBag.Price"/>
    <button type="submit">Create</button>
</form>
 
======================
@model List<Book>
<h1>Book List</h1>
<a asp-action="Create" >Create New Book</a>
<table>
    <thead>
        <th>Title</th>
        <th>Author</th>
        <th>Category</th>
        <th>Price</th>
    </thead>
    <table>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.BookId</td>
                <td>@item.Author</td>
                <td>@item.Category</td>
                <td>@item.Price</td>
            </tr>
           
        }
    </table>
</table>
================================
 
 
<h1>Book List</h1>
<a asp-action="Create" >Create New Book</a>
<table>
    <thead>
        <th>Title</th>
        <th>Author</th>
        <th>Category</th>
        <th>Price</th>
    </thead>
    <table>
        @foreach (var item in ViewBag.vari)
        {
            <tr>
                <td>@item.BookId</td>
                <td>@item.Author</td>
                <td>@item.Category</td>
                <td>@item.Price</td>
            </tr>
           
        }
    </table>
</table>
<h1>Book List</h1>
<a asp-action="Create" >Create New Book</a>
<table>
    <thead>
        <th>Title</th>
        <th>Author</th>
        <th>Category</th>
        <th>Price</th>
    </thead>
    <table>
        @foreach (var item in ViewBag.vari)
        {
            <tr>
                <td>@item.BookId</td>
                <td>@item.Author</td>
                <td>@item.Category</td>
                <td>@item.Price</td>
            </tr>
           
        }
    </table>
</table>
 
