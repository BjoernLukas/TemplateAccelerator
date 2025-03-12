using Microsoft.AspNetCore.Mvc;
using TemplateAcceleratorV1.DataUtility;
using TemplateAcceleratorV1.Models;
namespace TemplateAcceleratorV1.Controllers;

/// <summary>
/// This is a controller for managing Person entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly TemplateDbContext _templateDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersonController"/> class.
    /// </summary>
    /// <param name="templateDbContext">The template database context.</param>
    public PersonController(TemplateDbContext templateDbContext)
    {
        _templateDbContext = templateDbContext;
    }

    /// <summary>
    /// Retrieves a person by name.
    /// </summary>
    /// <param name="name">The name of the person.</param>
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
    /// Retrieves a person by ID.
    /// </summary>
    /// <param name="inputId">The ID of the person.</param>
    /// <returns>The person with the specified ID.</returns>
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
    /// Retrieves all persons.
    /// </summary>
    /// <returns>A list of all persons.</returns>
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
    /// Creates a new person.
    /// </summary>
    /// <remarks>If the name is missing, the ASP middleware will return a 400 Bad Request.</remarks>
    /// <param name="name">The name of the person.</param>
    /// <param name="age">The age of the person.</param>
    /// <param name="description">The description of the person.</param>
    /// <returns>The created person.</returns>
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
    /// Starts an import job, simulating a long running task.
    /// </summary>
    /// <returns>A response indicating that the task is being processed.</returns>
    [HttpPost("StartImportJob")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public IActionResult StartImportJob()
    {
        //TODO: Implement a long running task like a queue or a big import job
        //Remark we need a webhook or some kind of event to tell when the event is finished

        return Accepted("Task is being processed."); // returns 202 Accepted
    }

    /// <summary>
    /// Updates a person.
    /// </summary>
    /// <param name="personUpdate">The updated person object.</param>
    /// <returns>The updated person.</returns>
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

        // Update the person's details
        existingPerson.Name = personUpdate.Name;
        existingPerson.Age = personUpdate.Age;
        existingPerson.Description = personUpdate.Description;

        _templateDbContext.SaveChanges();

        return Ok(personUpdate);  // Return the updated person with 200 OK
    }

    //Remark: Patch was not on the list but I added it here.
    /// <summary>
    /// Updates the age of a person.
    /// </summary>
    /// <param name="id">The ID of the person.</param>
    /// <param name="inputAge">The new age of the person.</param>
    /// <returns>The updated person.</returns>
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

        return Ok(existingPerson);  // Return the updated person with 200 OK
    }

    /// <summary>
    /// Deletes a person by ID.
    /// </summary>
    /// <param name="id">The ID of the person to delete.</param>
    /// <returns>A response indicating the success of the deletion.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("DeletePerson/{id}")]
    public IActionResult DeletePerson(Guid id)
    {
        var existingPerson = _templateDbContext.Set<Person>().FirstOrDefault(p => p.PersonId == id);
        if (existingPerson == null)
        {
            return NotFound();  // 404 if person with given ID does not exist
        }

        _templateDbContext.Set<Person>().Remove(existingPerson);
        _templateDbContext.SaveChanges();

        return Ok();  // Return 200 OK
    }

    //TODO: Not sure how I will handle a 500 Internal Server Error, for example if the SQL server is down or something else goes wrong.
    // try-catch all  _templateDbContext.SaveChanges(); There must be something more elegant.  
}
