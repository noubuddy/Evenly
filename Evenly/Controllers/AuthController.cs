using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Evenly.Contexts;
using Evenly.Models;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

#nullable disable

namespace Evenly.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly Context _context;

        public AuthController(IConfiguration configuration, Context context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.User.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var us = _context.User.Find(id);
            if (us == null)
                return BadRequest("User not found");
            return Ok(us);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var us = _context.User.Find(id);
            if (us == null)
                return BadRequest("Data not found");

            _context.User.Remove(us);
            _context.SaveChanges();

            return Ok(_context.User.ToList());
        }

        [HttpPost("Register")]
        public IActionResult Register(UserDto request)
        {
            if (_context.User.Any(x => x.Username == request.Username))
                return BadRequest("User already exits"); 

            user = new User();

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = request.Email;
            user.CreatedAt = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

            _context.User.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserDto request)
        {
            if (_context.User.Any(x => x.Username != request.Username))
                return BadRequest("User not found");

            user = _context.User.First(x => x.Username == request.Username);

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            Jwt token = new Jwt {Token = CreateToken(user)};

            return Ok(JsonSerializer.Serialize(token));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {new Claim(ClaimTypes.Name, user.Username)};

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}