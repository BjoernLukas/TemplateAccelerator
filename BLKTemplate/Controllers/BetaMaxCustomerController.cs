using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.AspNetCore.Mvc;
namespace BetaMaxRMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BetaMaxCustomerController : ControllerBase
{
    private readonly BetaMaxDbContext _betaMaxDbContext;

    public BetaMaxCustomerController(BetaMaxDbContext templateDbContext)
    {
        _betaMaxDbContext = templateDbContext;
    }


    [HttpGet(Name = "ByName/{name}")]
    public IActionResult GetPersonByName(string name)
    {
        var person = _betaMaxDbContext.Set<BetaMaxCustomer>().Where(p => p.Name == name).FirstOrDefault();

        return person is null ? NotFound() : Ok(person);
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllPersons()
    {
        var allPersons = _betaMaxDbContext.Persons.ToList();

        return Ok(allPersons);
    }

    
    [HttpPost("CreateBlk")]
    public IActionResult CreateBlkPerson()
    {

        _betaMaxDbContext.Set<BetaMaxCustomer>().Add(new BetaMaxCustomer { Name = "Bjørn-Lukas", Remarks = "First test for this", Id = Guid.NewGuid() });


        _betaMaxDbContext.SaveChanges();

        return Ok();
    }


    [HttpPost]
    public IActionResult CreatePerson([FromBody] BetaMaxCustomer person)
    {
        _betaMaxDbContext.Set<BetaMaxCustomer>().Add(person);
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }




}
