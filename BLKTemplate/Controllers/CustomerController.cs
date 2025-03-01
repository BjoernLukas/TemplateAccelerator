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


    [HttpGet("GetTotalAmount")]
    public IActionResult GetTotalAmount()
    {


        return Ok();
    }

    [HttpGet("GetFrequentRenterPoints")]
    public IActionResult GetFrequentRenterPoints()
    {


        return Ok();
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

    [HttpPost("CreateDemoRentalsForAllMovies")]
    public IActionResult CreateDemoRentalsForAllMovies()
    {
        var customer = _betaMaxDbContext.Set<Customer>().First();
        var movies = _betaMaxDbContext.Set<Movie>().ToList();

        var startRentalTime = new DateTime(2025, 3, 1, 20, 0, 0);

        //Create a movieRental for each movie
        foreach (var movie in movies)
        {
            var movieRental = new MovieRental
            {
                MovieRelation = movie.Id,
                CustomerRelation = customer.Id,
                Start = startRentalTime
            };

            var handIndTime = CalculateHandInTimeFromMockData(movie.Title, startRentalTime);
            movieRental.UpdateStatus(handIndTime);
        }


        return Ok();
    }

    private DateTime CalculateHandInTimeFromMockData(string title, DateTime startRentalTime)
    {
        var rentalData = new Dictionary<string, int>
        {
                { "The Cell", 3 },
                { "The Tigger Movie", 3 },
                { "Plan 9 from Outer Space", 1 },
                { "8½", 2 },
                { "Eraserhead", 3 }
        };

        var daysRented = rentalData.ContainsKey(title) ? rentalData[title] : throw new Exception("No mock data for this movie");

        var endRentalTime = startRentalTime.AddDays(daysRented);

        return endRentalTime;
    }
}
