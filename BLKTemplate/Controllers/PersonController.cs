using Microsoft.AspNetCore.Mvc;
using TemplateAcceleratorV1.DataUtility;
using TemplateAcceleratorV1.Models;
namespace TemplateAcceleratorV1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonRepository _personRepository;
    private readonly TemplateDbContext _templateDbContext;

    public PersonController(IPersonRepository personRepository, TemplateDbContext templateDbContext)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
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

        //Todo Add some validation Checks

        return Ok();
    }

    [HttpGet("Test")]
    public IActionResult Test()
    {
        

        return Ok("Hello Swagger");
    }
}
