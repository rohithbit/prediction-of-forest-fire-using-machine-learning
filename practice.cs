program.cs
using dotnetapp.Services;
using dotnetapp.Repository;
using dotnetapp.Models;
 
var builder = WebApplication.CreateBuilder(args);
 
// Add Event services to the container.
 
builder.Services.AddControllers();
// builder.Services.AddSingleton<StudentService>();
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();
 
app.Urls.Add("http://0.0.0.0:8080");
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
 
services>BookServices.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Repository;
 
namespace dotnetapp.Services
{
    public class BookService : IBookService
    {
        private readonly BookRepository db;
        public BookService(){}
        public BookService(BookRepository db1){
            db = db1;
        }
        public List<Book> GetBooks() => db.GetBooks();
        public Book GetBook(int id) => db.GetBook(id);
        public Book SaveBook(Book book) => db.SaveBook(book);
        public Book UpdateBook(int id, Book book) => db.UpdateBook(id,book);
        public bool DeleteBook(int id) => db.DeleteBook(id);
    }
}
 
Services>IBookService
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Repository;
 
namespace dotnetapp.Services
{
    public interface IBookService
    {
        List<Book> GetBooks();
        Book GetBook(int id);
        Book SaveBook(Book book);
        Book UpdateBook(int id, Book book);
        bool DeleteBook(int id);
 
    }
}
 
Services>IOrderService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
using dotnetapp.Repository;
 
namespace dotnetapp.Services
{
    public interface IOrderService
    {
        List<Order> GetOrders();
        Order GetOrder(int id);
        Order SaveOrder(Order order);
        Order UpdateOrder(int id, Order order);
        bool DeleteOrder(int id);
    }
}
 
Services>OrderServices.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Repository;
using dotnetapp.Models;
 
namespace dotnetapp.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository db;
        public OrderService(){}
        public OrderService(OrderRepository db1){
            db = db1;
        }
        public List<Order> GetOrders() => db.GetOrders();
        public Order GetOrder(int id) => db.GetOrder(id);
        public Order SaveOrder(Order order) => db.SaveOrder(order);
        public Order UpdateOrder(int id, Order order) => db.UpdateOrder(id, order);
        public bool DeleteOrder(int id) => db.DeleteOrder(id);
    }
}
 
 
Repository>BookRepository.cs
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
 
namespace dotnetapp.Repository
{
    public class BookRepository
    {
        public readonly List<Book> _books = new List<Book>();
        public List<Book> GetBooks() => _books;
        public Book GetBook(int id) => _books.FirstOrDefault(b => b.BookId == id);
        public Book SaveBook(Book book){
            book.BookId = _books.Count > 0 ? _books.Max(b => b.BookId) + 1 : 1;
            _books.Add(book);
            return book;
        }
        public Book UpdateBook(int id, Book book){
            var existingBook = GetBook(id);
            if(existingBook != null){
                existingBook.BookName = book.BookName;
                existingBook.Category = book.Category;
                existingBook.Price = book.Price;
            }
            return existingBook;
        }
        public bool DeleteBook(int id){
            var book = GetBook(id);
            if(book != null){
                _books.Remove(book);
                return true;
            }
            return false;
        }
    }
}
 
Repository>OrderRepository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;
 
namespace dotnetapp.Repository
{
    public class OrderRepository
    {
        public readonly List<Order> _orders = new List<Order>();
        public List<Order> GetOrders() => _orders;
        public Order GetOrder(int id) => _orders.FirstOrDefault(o => o.OrderId == id);
        public Order SaveOrder(Order order){
            order.OrderId = _orders.Count > 0 ? _orders.Max(o => o.OrderId) + 1 : 1;
            _orders.Add(order);
            return order;
        }
        public Order UpdateOrder(int id, Order order){
            var existingOrder = GetOrder(id);
            if(existingOrder != null){
                existingOrder.CustomerName = order.CustomerName;
                existingOrder.TotalAmount = order.TotalAmount;
            }
            return existingOrder;
        }
        public bool DeleteOrder(int id){
            var order = GetOrder(id);
            if(order != null){
                _orders.Remove(order);
                return true;
            }
            return false;
        }
    }
}
 
Controller>BookController
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetapp.Services;
using dotnetapp.Models;
 
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService db;
        public BookController(IBookService db1){
            db = db1;
        }
        [HttpGet]
        public IActionResult GetAllBooks(){
            return Ok(db.GetBooks());
        }
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id){
            var book = db.GetBook(id);
            if(book == null) return NotFound();
            return Ok(book);
        }
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book){
            if(book == null) return BadRequest();
            var createBook = db.SaveBook(book);
            return Ok(createBook);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book){
            if(book == null) return BadRequest();
            var updatedBook = db.UpdateBook(id, book);
            if(updatedBook == null) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id){
            var isDeleted = db.DeleteBook(id);
            // if(!isDeleted){
            //     return NotFound();
            // }
            return NoContent();
        }
    }
}
 
 
Controllers>OrderController.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Services;
 
namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService db;
        public OrderController(IOrderService db1)
        {
            db = db1;
        }
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(db.GetOrders());
        }
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = db.GetOrder(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpPost]
        public IActionResult AddOrder([FromBody] Order order)
        {
            if (order == null) return BadRequest();
            var createOrder = db.SaveOrder(order);
            return Ok(createOrder);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order order)
        {
            if (order == null) return BadRequest();
            var updatedOrder = db.UpdateOrder(id, order);
            if (updatedOrder == null) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var isDeleted = db.DeleteOrder(id);
            // if (!isDeleted){
            //     return NotFound();
            // }
            return NoContent();
        }
    }
}
 
