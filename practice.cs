using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
namespace dotnetapp.Models
{
public class Plant
{
    [Key]
    public int PlantId { get; set; }
    [Required]
 
    public string Name { get; set; } = string.Empty;
    [Required]
     public string ScientificName { get; set; } = string.Empty;
 
    [Required]
 
    public string Description { get; set; } = string.Empty;
    [Required]
 
    public double Price { get; set; }
 
}
}
 
 
 
plant model
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
        using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
public class User
 
{
    [Key]
    public long UserId { get; set; }
    [Required]
    [EmailAddress]
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
 
 
 
USer Model
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dotnetapp.Models
{
public class LoginModel
{
    [Required]
 
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
 
}
}
 
   
 
Login Model
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace dotnetapp.Models
{
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
 
    {
    }
    public DbSet<User> Users { get; set; }
 
    public DbSet<Plant> Plants { get; set; }
 
}
}
 
 
 
ApplicationDbContext
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
 
using dotnetapp.Services;
 
using Microsoft.AspNetCore.Mvc;
namespace dotnetapp.Controllers
{
   
 
[ApiController]
 
[Route("api")]
 
public class AuthenticationController : ControllerBase
 
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthenticationController> _logger;
    public AuthenticationController(
        IAuthService authService,
        ApplicationDbContext context,
 
        ILogger<AuthenticationController> logger)
    {
        _authService = authService;
        _context = context;
        _logger = logger;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await _authService.Login(model);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Invalid email or password" });
            }
            return Ok(new { token = token });
        }
 
        catch (Exception ex)
 
        {
 
            _logger.LogError(ex, "Error occurred during login");
 
            return BadRequest(new { message = "An error occurred during login" });
 
        }
 
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user.UserRole != "Admin" && user.UserRole != "Customer")
            {
                return BadRequest(new { message = "Invalid user role. Allowed roles are Admin or Customer" });
            }
            var result = await _authService.Registration(user, user.UserRole);
 
            if (result != "User registered successfully")
            {
                return BadRequest(new { message = result });
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during registration");
 
            return BadRequest(new { message = "An error occurred during registration" });
 
        }
 
    }
 
}
}
 
       
 
AuthenticationConroller
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace dotnetapp.Controllers
{
 
    [ApiController]
 
    [Route("api/plants")]
 
    [Authorize]
    public class PlantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
   public PlantController(ApplicationDbContext context)
        {
            _context = context;
 
        }
        [HttpGet]
        public async Task<IActionResult> Get()
 
        {
            var plants = await _context.Plants.ToListAsync();
 
            return Ok(plants);
        }
 
 
        [HttpGet("{id}")]
 
        public async Task<IActionResult> GetById(int id)
 
        {
 
            var plant = await _context.Plants.FindAsync(id);
 
 
            if (plant == null)
 
            {
 
                return NotFound();
 
            }
 
   return Ok(plant);
 
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
 
        public async Task<IActionResult> Post([FromBody] Plant plant)
        {
 
            if (plant == null)
{
 
                return BadRequest(new { message = "Plant data is required" });
 
            }
            _context.Plants.Add(plant);
 
            await _context.SaveChangesAsync();
 
 
            return CreatedAtAction(nameof(GetById), new { id = plant.PlantId }, plant);
 
        }
 
    }
}
 
 
PlantController
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotnetapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace dotnetapp.Services
 
{
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
       private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;
    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
 
        _configuration = configuration;
 
        _passwordHasher = new PasswordHasher<User>();
 
    }
    public async Task<string> Registration(User model, string role)
 
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
 
        if (existingUser != null)
 
        {
 
            return "User already exists";
 
        }
        model.UserRole = role;
        model.Password = _passwordHasher.HashPassword(model, model.Password);
        return "User registered successfully";
 
    }
    public async Task<string?> Login(LoginModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
        {
            return null;
        }
        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
 
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
 
            new Claim(ClaimTypes.Email, user.Email),
 
            new Claim(ClaimTypes.Role, user.UserRole),
 
            new Claim("UserId", user.UserId.ToString())
        };
        return GenerateToken(claims);
    }
    public string GenerateToken(IEnumerable<Claim> claims)
 
    {
 
        var key = _configuration["Jwt:Key"] ?? "ThisIsASecretKeyForJwtAuthentication12345";
 
        var issuer = _configuration["Jwt:Issuer"] ?? "dotnetapp";
 
        var audience = _configuration["Jwt:Audience"] ?? "dotnetapp_users";
 
        var duration = int.TryParse(_configuration["Jwt:DurationInMinutes"], out var mins) ? mins : 60;
 
 
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
 
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
 
 
        var token = new JwtSecurityToken(
 
            issuer: issuer,
 
            audience: audience,
 
            claims: claims,
 
            expires: DateTime.UtcNow.AddMinutes(duration),
 
            signingCredentials: credentials
 
        );
 
 
        return new JwtSecurityTokenHandler().WriteToken(token);
 
    }
 
}
}
 
 
 
AuthService
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
     using System.Security.Claims;
using dotnetapp.Models;
 
namespace dotnetapp.Services
{
public interface IAuthService
 
{
       Task<string> Registration(User model, string role);
   Task<string?> Login(LoginModel model);
 
    string GenerateToken(IEnumerable<Claim> claims);
 
}
}
   
 
 
IAuthService
 
using System.Text;
 
using dotnetapp.Models;
 
using dotnetapp.Services;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
 
using Microsoft.EntityFrameworkCore;
 
using Microsoft.IdentityModel.Tokens;
 
var builder = WebApplication.CreateBuilder(args);
 
builder.WebHost.UseUrls("http://0.0.0.0:8080");
builder.Services.AddControllers();
 
builder.Services.AddEndpointsApiExplorer();
 
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 
options.UseSqlServer("user id=sa;password=examlyMssql@123;database=appdb;server=localhost;persist security info=false;trusted_connection=false;encrypt=false;"));
 
builder.Services.AddScoped<IAuthService, AuthService>();
 
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsASecretKeyForJwtAuthentication12345";
 
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
 
app.UseSwagger();
app.UseSwaggerUI();
 
app.UseAuthentication();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 
 
Program.cs
 
