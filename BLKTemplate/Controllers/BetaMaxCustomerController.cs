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
    public IActionResult GetCustomerByName(string name)
    {
        var person = _betaMaxDbContext.Set<BetaMaxCustomer>().Where(p => p.Name == name).FirstOrDefault();

        return person is null ? NotFound() : Ok(person);
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllCustomers()
    {
        var result = _betaMaxDbContext.Persons.ToList();

        return Ok(result);
    }

    
    [HttpPost("CreateDemoCustomer")]
    public IActionResult CreateDemoCustomer()
    {
        var customer = new BetaMaxCustomer
        {
            Name = "John Doe",
            Remarks = "Frequent renter",
            Gender = GenderInfo.Male
        };

        _betaMaxDbContext.Set<BetaMaxCustomer>().Add(customer);
        _betaMaxDbContext.SaveChanges();

        return Ok(customer);
    }


    [HttpPost]
    public IActionResult CreateCustomer([FromBody] BetaMaxCustomer customer)
    {
        _betaMaxDbContext.Set<BetaMaxCustomer>().Add(customer);
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }




}
