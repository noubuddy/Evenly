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
        private static IWebHostEnvironment _enviroment;
        private FileUpload obj;
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
            // if (!Directory.Exists(_enviroment.WebRootPath + "\\Resources\\Images\\"))
            //     Directory.CreateDirectory(_enviroment.WebRootPath + "\\Resources\\Images\\");

            // using (FileStream fs = System.IO.File.Create(_enviroment.WebRootPath + "\\Resources\\Images\\" + obj.file.FileName))
            // {
            //     obj.file.CopyTo(fs);
            //     fs.Flush();
            //     data.Image = fs.ToString();
            // }

            data.CreatedAt = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            _context.Data.Add(data);
            _context.SaveChanges();
            return Ok(_context.Data.ToList());
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