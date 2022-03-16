﻿using Microsoft.AspNetCore.Mvc;
using Evenly.Models;
using Evenly.Contexts;

namespace Evenly.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly Context _context;
        public DataController(Context context)
        {
            _context = context;
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
                return BadRequest("Data not found.");
            return Ok(dt);
        }

        [HttpPost]
        public IActionResult AddData(Data data)
        {
            _context.Data.Add(data);
            _context.SaveChanges();
            return Ok(_context.Data.ToList());
        }

        [HttpPut]
        public IActionResult UpdateData(Data request)
        {
            var dt = _context.Data.Find(request.Id);
            if (dt == null)
                return BadRequest("Data not found.");

            dt.Title = request.Title;
            dt.Description = request.Description;
            dt.Coordinates = request.Coordinates;
            dt.Time = request.Time;

            _context.SaveChanges();

            return Ok(_context.Data.ToList());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dt = _context.Data.Find(id);
            if (dt == null)
                return BadRequest("Data not found.");

            _context.Data.Remove(dt);

            _context.SaveChanges();

            return Ok(_context.Data.ToList());
        }

    }
}