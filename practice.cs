senco token program
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
         
namespace dotnetapp.Controllers
 
{
 
    using dotnetapp.Models;
 
    using dotnetapp.Services;
 
    [ApiController]
 
    [Route("api")]
 
    public class AuthenticationController : ControllerBase
 
    {
 
    private readonly IAuthService _authService;
 
    private readonly ApplicationDbContext _context;
 
    private readonly ILogger<AuthenticationController> _logger;
 
    public AuthenticationController(IAuthService authService, ApplicationDbContext context, ILogger<AuthenticationController> logger)
 
    {
 
    _authService = authService;
 
    _context = context;
 
    _logger = logger;
 
    }
 
    [HttpPost("login")]
 
    [AllowAnonymous]
 
    public IActionResult Login([FromBody] LoginModel model)
 
    {
 
    try
 
    {
 
    if (!ModelState.IsValid)
 
    {
 
    return BadRequest("Invalid payload");
 
    }
 
    var token = _authService.Login(model);
 
    if (string.IsNullOrEmpty(token))
 
    {
 
    return BadRequest("Invalid email or password");
 
    }
 
    return Ok(new { Token = token });
 
    }
 
    catch (Exception ex)
 
    {
 
    _logger.LogError(ex, "Error occurred during login");
 
    return BadRequest("An error occurred during authentication");
 
    }
 
    }
 
    [HttpPost("register")]
 
    [AllowAnonymous]
 
    public IActionResult Register([FromBody] User user)
 
    {
 
    try
 
    {
 
    if (!ModelState.IsValid)
 
    {
 
    return BadRequest("Invalid payload");
 
    }
 
    if (user.UserRole != "Admin" && user.UserRole != "Customer")
 
    {
 
    return BadRequest("Invalid user role");
 
    }
 
    var result = _authService.Registration(user, user.UserRole);
 
    if (result != "User registered successfully")
 
    {
 
    return BadRequest(result);
 
    }
 
    _context.Users.Add(user);
 
    _context.SaveChanges();
 
    return Ok(new { Message = "User registered successfully" });
 
    }
 
    catch (Exception ex)
 
    {
 
    _logger.LogError(ex, "Error occurred during registration");
 
    return BadRequest("An error occurred during registration");
 
    }
 
    }
 
    }
 
    }
 
 
authcontroller  up
 
 
book controller
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
 
 
using Microsoft.IdentityModel.Tokens;
namespace dotnetapp.Controllers
{
            [ApiController]
 
    [Route("api/books")]
 
    [Authorize]
 
    public class BookController : ControllerBase
 
    {
 
    private readonly ApplicationDbContext _context;
 
    public BookController(ApplicationDbContext context)
 
    {
 
    _context = context;
 
    }
 
    [HttpGet]
 
    public async Task<IActionResult> Get()
 
    {
 
    var books = await _context.Books.ToListAsync();
 
    return Ok(books);
 
    }
 
    [HttpPost]
 
    [Authorize(Roles = "Admin")]
 
    public async Task<IActionResult> Post([FromBody] Book book)
 
    {
 
    if (book == null)
 
    {
 
    return BadRequest("Book data is required");
 
    }
 
    _context.Books.Add(book);
 
    await _context.SaveChangesAsync();
 
    return CreatedAtAction(nameof(Get), new { id = book.BookId }, book);
 
    }
 
    }
 
}
 
 
 
 
appdb
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
namespace dotnetapp.Models
{
       
    public class ApplicationDbContext : DbContext
 
    {
 
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
 
    {
 
    }
 
    public DbSet<User> Users { get; set; }
 
    public DbSet<Book> Books { get; set; }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
 
    {
 
    base.OnModelCreating(modelBuilder);
 
    modelBuilder.Entity<User>()
 
    .HasIndex(x => x.Email)
 
    .IsUnique();
 
    modelBuilder.Entity<User>()
 
    .Property(x => x.UserId)
 
    .ValueGeneratedOnAdd();
 
    modelBuilder.Entity<Book>()
 
    .Property(x => x.BookId)
 
    .ValueGeneratedOnAdd();
 
    }
 
    }
 
}
 
 
book.cs
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
namespace dotnetapp.Models
{   public class Book
 
    {
 
    [Key]
 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
 
    public int BookId { get; set; }
 
    // Alias for payload/tests if they use Bookld instead of BookId
 
    [NotMapped]
 
    public int Bookld
 
    {
 
    get { return BookId; }
 
    set { BookId = value; }
 
    }
 
    [Required]
 
    public string Title { get; set; } = string.Empty;
 
    [Required]
 
    public string Author { get; set; } = string.Empty;
 
    [Required]
 
    public string Description { get; set; } = string.Empty;
 
    [Required]
 
    public double Price { get; set; }
 
    }
    }
       
   
 
 
loginmodl.cs
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
 
 
 
 
   
 
 
 
namespace dotnetapp.Models
{
    public class User
    {
       
    [Key]
 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
 
    public long UserId { get; set; }
 
    // Alias for payload/tests if they use Userld instead of UserId
 
    [NotMapped]
 
    public long Userld
 
    {
 
    get { return UserId; }
 
    set { UserId = value; }
 
    }
 
    [Required]
 
    public string Email { get; set; } = string.Empty;
 
    [Required]
 
    public string Password { get; set; } = string.Empty;
 
    [Required]
 
    public string Username { get; set; } = string.Empty;
 
    [Required]
 
    public string MobileNumber { get; set; } = string.Empty;
 
    [Required]
 
    public string UserRole { get; set; } = string.Empty;
 
    }
    }
 
 
user cs uppp
 
auth service
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
using dotnetapp.Models;
 
namespace dotnetapp.Services
{
    public class AuthService : IAuthService
 
    {
 
    private readonly ApplicationDbContext _context;
 
    private readonly IConfiguration _configuration;
 
    public AuthService(ApplicationDbContext context, IConfiguration configuration)
 
    {
 
    _context = context;
 
    _configuration = configuration;
 
    }
 
    public string Registeraton(User model, string role)
 
    {
 
    return Registration(model, role);
 
    }
 
    public string Registration(User model, string role)
 
    {
 
    if (model == null)
 
    {
 
    return "Invalid user data";
 
    }
 
    if (string.IsNullOrWhiteSpace(role))
 
    {
 
    return "Invalid role";
 
    }
 
    if (_context.Users.Any(x => x.Email == model.Email))
 
    {
 
    return "User already exists";
 
    }
 
    model.UserRole = role;
 
    return "User registered successfully";
 
    }
 
    public string Login(LoginModel model)
 
    {
 
    if (model == null)
 
    {
 
    return string.Empty;
 
    }
 
    var user = _context.Users.FirstOrDefault(x =>
 
    x.Email == model.Email && x.Password == model.Password);
 
    if (user == null)
 
    {
 
    return string.Empty;
 
    }
 
    var claims = new List<Claim>
 
    {
 
    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
 
    new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
 
    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
 
    new Claim(ClaimTypes.Role, user.UserRole ?? string.Empty)
 
    };
 
    return GenerateToken(claims);
 
    }
 
    public string GenerateToken(IEnumerable<Claim> claims)
 
    {
 
    var key = _configuration["Jwt:Key"] ?? "ThisIsMyVerySecureJwtKeyForBooksManagement12345";
 
    var issuer = _configuration["Jwt:Issuer"] ?? "dotnetapp";
 
    var audience = _configuration["Jwt:Audience"] ?? "dotnetapp_users";
 
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
 
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
 
    var token = new JwtSecurityToken(
 
    issuer: issuer,
 
    audience: audience,
 
    claims: claims,
 
    expires: DateTime.UtcNow.AddHours(1),
 
    signingCredentials: credentials
 
    );
 
    return new JwtSecurityTokenHandler().WriteToken(token);
 
    }
 
    }
 
}
 
iauthservice
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
using System.ComponentModel.DataAnnotations.Schema;
 
using System.IdentityModel.Tokens.Jwt;
 
using System.Security.Claims;
 
using System.Text;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
namespace dotnetapp.Services
 
{
 
    using dotnetapp.Models;
 
    public interface IAuthService
 
    {
 
    string Registeraton(User model, string role);
 
    string Registration(User model, string role);
 
    string Login(LoginModel model);
 
    string GenerateToken(IEnumerable<Claim> claims);
 
    }
 
 
    }
 
 
Mohit Rawal(IN8192) changed the group name to Komo.

 
 
namespace dotnetapp
 
{
 
    using System.Text;
 
    using dotnetapp.Models;
 
    using dotnetapp.Services;
 
    using Microsoft.AspNetCore.Authentication.JwtBearer;
 
    using Microsoft.EntityFrameworkCore;
 
    using Microsoft.IdentityModel.Tokens;
 
    public class Program
 
    {
 
    public static void Main(string[] args)
 
    {
 
    var builder = WebApplication.CreateBuilder(args);
 
    // Run on port 8080
 
    builder.WebHost.UseUrls("http://0.0.0.0:8080");
 
    // Add configuration values in-memory so appsettings.json is not required
 
    var configValues = new Dictionary<string, string?>
 
    {
 
    { "ConnectionStrings:DefaultConnection", "User ID=sa;password=examlyMssql@123;server=localhost;Database=appdb;trusted_connection=false;Persist Security Info=False;Encrypt=False" },
 
    { "Jwt:Key", "ThisIsMyVerySecureJwtKeyForBooksManagement12345" },
 
    { "Jwt:Issuer", "dotnetapp" },
 
    { "Jwt:Audience", "dotnetapp_users" }
 
    };
 
    builder.Configuration.AddInMemoryCollection(configValues);
 
    builder.Services.AddControllers();
 
    builder.Services.AddEndpointsApiExplorer();
 
    builder.Services.AddSwaggerGen();
 
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
    builder.Services.AddScoped<IAuthService, AuthService>();
 
    var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsMyVerySecureJwtKeyForBooksManagement12345";
 
    var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "dotnetapp";
 
    var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "dotnetapp_users";
 
    builder.Services.AddAuthentication(options =>
 
    {
 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 
    })
 
    .AddJwtBearer(options =>
 
    {
 
    options.RequireHttpsMetadata = false;
 
    options.SaveToken = true;
 
    options.TokenValidationParameters = new TokenValidationParameters
 
    {
 
    ValidateIssuer = true,
 
    ValidateAudience = true,
 
    ValidateLifetime = true,
 
    ValidateIssuerSigningKey = true,
 
    ValidIssuer = jwtIssuer,
 
    ValidAudience = jwtAudience,
 
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
 
    };
 
    });
 
    builder.Services.AddAuthorization();
 
    var app = builder.Build();
 
    // Create database automatically
 
    using (var scope = app.Services.CreateScope())
 
    {
 
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
 
    db.Database.EnsureCreated();
 
    }
 
    app.UseSwagger();
 
    app.UseSwaggerUI();
 
    app.UseAuthentication();
 
    app.UseAuthorization();
 
    app.MapControllers();
 
    app.Run();
 
    }
 
    }
 
}
 
