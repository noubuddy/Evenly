using Microsoft.AspNetCore.Mvc;
using Evenly.Models;
using System.Security.Cryptography;
using Evenly.Contexts;

namespace Evenly.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto request){
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto requset){

            if (user.Username != requset.Username) {
                return BadRequest("User not found.");
            }

            return Ok("Success");
        }
    }
}