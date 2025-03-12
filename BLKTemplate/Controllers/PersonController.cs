using Microsoft.AspNetCore.Mvc;
using TemplateAcceleratorV1.DataUtility;
using TemplateAcceleratorV1.Models;
namespace TemplateAcceleratorV1.Controllers;

/// <summary>
/// This is a controller for Person Entity
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly TemplateDbContext _templateDbContext;
    public PersonController(TemplateDbContext templateDbContext)
    {
        _templateDbContext = templateDbContext;
    }

    /// <summary>
    /// This will return a personUpdate by name
    /// </summary>
    /// <param name="name"></param>    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet(Name = "Name/{name}")]
    public IActionResult GetPersonByName(string name)
    {
        var person = _templateDbContext.Set<Person>().Where(p => p.Name == name).FirstOrDefault();

        return person is null ? NotFound() : Ok(person);
    }

    /// <summary>
    /// Returns a personUpdate by id (For testing "9b37bbe0-e7ea-40bb-a986-11835fbfa0ae"
    /// </summary>
    /// <param name="inputId"></param>    
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("id/{inputId}")]
    public IActionResult GetPersonById(string inputId)
    {
        if (Guid.TryParse(inputId, out var guid) is false)
        { return BadRequest("Invalid GUID format."); }

        var person = _templateDbContext.Set<Person>().FirstOrDefault(p => p.PersonId == guid);
        return person is null ? NotFound() : Ok(person);
    }

    //Remark could also do GetPersonById(Guid id)
    //with [HttpGet("id/{id:guid}")] then the middleware will check if input is a valid guild and if not respond with 404 Not found 

    /// <summary>
    /// This will return all persons, useful when testing the other endpoints 
    /// </summary>
    /// <returns>List of Person </returns>
    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAllPersons()
    {
        var allPersons = _templateDbContext.Persons.ToList();
        if (allPersons.Any() is false)
        { return NoContent(); }

        return Ok(allPersons);
    }

    /// <summary>
    /// Creates a new Person.
    /// </summary>
    /// <remarks> if name is missing ASP middleware will return return a 400 Bad Request </remarks>
    /// <param name="name"></param>
    /// <param name="age"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("CreatePerson")]
    public IActionResult CreatePerson(string name, int? age, string? description)
    {
        var person = new Person
        {
            Name = name,
            Age = age,
            Description = description,
        };


        _templateDbContext.Set<Person>().Add(person);
        _templateDbContext.SaveChanges();

        return CreatedAtAction("CreatePerson", person);  //Status 201 Created
    }

    /// <summary>
    /// Simulates a long running task,
    /// </summary>
    /// <returns></returns>
    [HttpPost("StartImportJob")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]    
    public IActionResult StartImportJob()
    {
        //TODO: Implement a long running task like a que or a big import job
        //Remark we need a webhook or some kind of event to tell with the event is finished

        return Accepted("Task is being processed."); // returns 202 Accepted
    }

    /// <summary>
    /// Updates the Person
    /// </summary>
    /// <param name="personUpdate"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("UpdatePerson")]
    public IActionResult UpdatePerson([FromBody] Person personUpdate)
    {
        //TODO: improve this with a PersonRequest model like UpdatePerson(Guid id, [FromBody] PersonRequest personRequest) 

        var existingPerson = _templateDbContext.Set<Person>().FirstOrDefault(p => p.PersonId == personUpdate.PersonId);
        if (existingPerson == null)
        {
            return NotFound();  // 404 if personUpdate with given ID does not exist
        }

        // Update the personUpdate's details
        existingPerson.Name = personUpdate.Name;
        existingPerson.Age = personUpdate.Age;
        existingPerson.Description = personUpdate.Description;

        _templateDbContext.SaveChanges();

        return Ok(personUpdate);  // Return the updated person with 200 OK
    }

    //Remark: Patch was not on the list but I added it here.
    /// <summary>
    /// Updates the age
    /// </summary>
    /// <param name="id"></param>
    /// <param name="inputAge"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPatch("UpdatePersonAge/{id}/{inputAge}")]
    public IActionResult UpdatePersonAge(Guid id, int inputAge)
    {
        var existingPerson = _templateDbContext.Set<Person>().FirstOrDefault(p => p.PersonId == id);
        if (existingPerson == null)
        {
            return NotFound();  // 404 if personUpdate with given ID does not exist
        }
        
        existingPerson.Age = inputAge;

        _templateDbContext.SaveChanges();

        return Ok(existingPerson);  // Return the updated personUpdate with 200 OK
    }



    //TODO: Not sure how I will do a 500 Internal Server Error, for if the SQL server is down or something else goes wrong.
    // tryCatch all  _templateDbContext.SaveChanges(); There must be something more elegant.  

}
