using Microsoft.AspNetCore.Mvc;
using Evenly.Contexts;
using Evenly.Models;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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

        [HttpPost("register")]
        public IActionResult Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            User _user = new User();
            _user.Username = user.Username;
            _user.PasswordHash = user.PasswordHash;
            _user.PasswordSalt = user.PasswordSalt;

            _context.User.Add(_user);

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto requset)
        {
            if (user.Username != requset.Username)
                return BadRequest("User not found.");

            if (!VerifyPasswordHash(requset.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            string token = CreateToken(user);
            
            return Ok(token);
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