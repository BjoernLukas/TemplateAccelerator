using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.AspNetCore.Mvc;
namespace BetaMaxRMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly BetaMaxDbContext _betaMaxDbContext;

    public CustomerController(BetaMaxDbContext betaMaxDbContext)
    {
        _betaMaxDbContext = betaMaxDbContext;
    }


    [HttpGet("ByName/{name}")]
    public IActionResult GetCustomerByName(string name)
    {
        var customer = _betaMaxDbContext.Set<Customer>().Where(p => p.Name == name).FirstOrDefault();
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllCustomers()
    {
        var result = _betaMaxDbContext.Customer.ToList();

        return Ok(result);
    }

    
    [HttpPost("CreateDemoCustomer")]
    public IActionResult CreateDemoCustomer()
    {
        var customer = new Customer
        {
            Name = "John Doe",
            Remarks = "Frequent renter",
            Gender = GenderInfo.Male
        };

        _betaMaxDbContext.Set<Customer>().Add(customer);
        _betaMaxDbContext.SaveChanges();

        return Ok(customer);
    }


    [HttpPost]
    public IActionResult CreateCustomer([FromBody] Customer customer)
    {
        _betaMaxDbContext.Set<Customer>().Add(customer);
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }




}
