using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Evenly.Models;
using Evenly.Contexts;
using Renci.SshNet;

namespace Evenly.Controllers;

[Route("[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly DataContext _context;
    public DataController(DataContext context)
    {
        _context = context;
    }
    
    private static List<DataModel> data = new List<DataModel>
    {
        new DataModel{Id = 0, Number = 130, Text = "Sweden"},
        new DataModel{Id = 1, Number = 986, Text = "Cat and dog"}
    };

    
    [HttpGet]
    public ActionResult<List<DataModel>> Get()
    {
        return Ok(data.ToList());
    }
    

    [HttpGet("{id}")]
    public ActionResult<DataModel> Get(int id)
    {
        var dt = data.Find(q => q.Id == id);
        if (dt == null)
            return BadRequest("Data not found.");
        return Ok(dt);
    }

    [HttpPost]
    public ActionResult<List<DataModel>> AddData(DataModel dt)
    {
        data.Add(dt);
        return Ok(data);
    }

    [HttpPut]
    public ActionResult<List<DataModel>> UpdateData(DataModel request)
    {
        var dt = data.Find(q => q.Id == request.Id);
        if (dt == null)
            return BadRequest("Data not found.");

        dt.Number = request.Number;
        dt.Text = request.Text;

        return Ok(data);
    }

    [HttpDelete("{id}")]
    public ActionResult<DataModel> Delete(int id)
    {
        var dt = data.Find(q => q.Id == id);
        if (dt == null)
            return BadRequest("Data not found.");

        data.Remove(dt);
        return Ok(dt);
    }
    
}