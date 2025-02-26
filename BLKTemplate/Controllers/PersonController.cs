using Microsoft.AspNetCore.Mvc;
using TemplateAcceleratorV1.DataUtility;
using TemplateAcceleratorV1.Models;
namespace TemplateAcceleratorV1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly TemplateDbContext _templateDbContext;

    public PersonController(TemplateDbContext templateDbContext)
    {
        _templateDbContext = templateDbContext;
    }


    [HttpGet(Name = "ByName/{name}")]
    public IActionResult GetPersonByName(string name)
    {
        var person = _templateDbContext.Set<Person>().Where(p => p.Name == name).FirstOrDefault();

        return person is null ? NotFound() : Ok(person);
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllPersons()
    {
        var allPersons = _templateDbContext.Persons.ToList();

        return Ok(allPersons);
    }

    [HttpPost("CreateBlk")]
    public IActionResult CreateBlkPerson()
    {

        _templateDbContext.Set<Person>().Add(new Person { Name = "BLK", Age = 38, Description = "First test for this", PersonId = Guid.NewGuid() });


        _templateDbContext.SaveChanges();

        return Ok();
    }


    [HttpPost]
    public IActionResult CreatePerson([FromBody] Person person)
    {
        _templateDbContext.Set<Person>().Add(person);
        _templateDbContext.SaveChanges();

        return Ok();
    }




}
