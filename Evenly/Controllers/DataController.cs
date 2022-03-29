using Microsoft.AspNetCore.Mvc;
using Evenly.Models;
using Evenly.Contexts;

namespace Evenly.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly Context _context;
        private static IWebHostEnvironment? _enviroment;
        // private FileUpload? obj;
        public DataController(Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Data.ToList());
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var dt = _context.Data.Find(id);
            if (dt == null)
                return BadRequest("Data not found");
            return Ok(dt);
        }

        [HttpPost]
        public IActionResult AddData(Data data)
        {
            data.CreatedAt = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            _context.Data.Add(data);
            _context.SaveChanges();
            return Ok(_context.Data.ToList());
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromForm] FileUpload image)
        {
            // upload image
            if (image.Image.Length > 0)
            {
                if (!Directory.Exists(_enviroment!.WebRootPath + "\\Upload\\"))
                    Directory.CreateDirectory(_enviroment.WebRootPath + "\\Upload\\");

                using (FileStream fs = System.IO.File.Create(_enviroment.WebRootPath + "\\Upload\\" + image.Image.FileName))
                {
                    image.Image.CopyTo(fs);
                    fs.Flush();
                    return Ok(_enviroment.WebRootPath + "\\Upload\\" + image.Image.FileName);
                }
            }
            return BadRequest("Uploading failed");
        }

        [HttpPut]
        public IActionResult UpdateData(Data request)
        {
            var dt = _context.Data.Find(request.Id);
            if (dt == null)
                return BadRequest("Data not found");

            dt.Title = request.Title;
            dt.Description = request.Description;
            dt.Coordinates = request.Coordinates;
            dt.CreatedAt = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

            _context.SaveChanges();

            return Ok(_context.Data.ToList());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dt = _context.Data.Find(id);
            if (dt == null)
                return BadRequest("Data not found");

            _context.Data.Remove(dt);

            _context.SaveChanges();

            return Ok(_context.Data.ToList());
        }

    }
}