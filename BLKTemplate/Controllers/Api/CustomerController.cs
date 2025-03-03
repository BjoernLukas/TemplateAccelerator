using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.AspNetCore.Mvc;
namespace BetaMaxRMS.Controllers.Api;

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

    //Endpoints for: GetTotalAmount and GetFrequentRenterPoints, could be added here in the future. 

    //Todo: this could be moved to a separate controller
    [HttpGet("GetAllMovieRentals")]
    public IActionResult GetAllMovieRentals()
    {
        var movieRentals = _betaMaxDbContext.Set<MovieRental>().ToList();
        return Ok(movieRentals);
    }
}
