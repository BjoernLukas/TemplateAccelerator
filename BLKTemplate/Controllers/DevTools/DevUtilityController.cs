using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using BetaMaxRMS.Services;
using Microsoft.AspNetCore.Mvc;
namespace BetaMaxRMS.Controllers.DevTools;

[ApiController]
[Route("api/[controller]")]
public class DevUtilityController : ControllerBase
{
    private readonly BetaMaxDbContext _betaMaxDbContext;
    private readonly IMovieRentalCalculationService _movieRentalCalculationService;

    public DevUtilityController(BetaMaxDbContext betaMaxDbContext, IMovieRentalCalculationService movieRentalCalculationService)
    {
        _betaMaxDbContext = betaMaxDbContext;
        _movieRentalCalculationService = movieRentalCalculationService;
    }

    [HttpPost("CreateDemoMovies")]
    public IActionResult CreateDemoMovies()
    {
        var movies = new List<Movie>
        {
            new() { Title = "The Cell", PriceCode = PriceCode.NewRelease },
            new() { Title = "The Tigger Movie", PriceCode = PriceCode.Childrens },
            new() { Title = "Plan 9 from Outer Space", PriceCode = PriceCode.Regular },
            new() { Title = "8 1/2", PriceCode = PriceCode.Regular },
            new() { Title = "Eraserhead", PriceCode = PriceCode.Regular }
        };

        _betaMaxDbContext.AddRange(movies);
        _betaMaxDbContext.SaveChanges();

        return Ok("Demo Movies created");
    }

    [HttpPost("CreateDemoCustomer")]
    public IActionResult CreateDemoCustomer()
    {
        var customer = new Customer
        {
            Name = "John Developer",
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

        var allMovieRentals = new List<MovieRental>();
        //Create a movieRental for each movie
        foreach (var movie in movies)
        {
            var movieRental = new MovieRental
            {
                MovieRelation = movie.Id,
                CustomerRelation = customer.Id,
                Start = startRentalTime
            };

            var handIndTime = SimulateHandInTimeFromMockData(movie.Title, startRentalTime);
            movieRental.UpdateWhenHandIn(handIndTime);
            allMovieRentals.Add(movieRental);
        }

        _betaMaxDbContext.AddRange(allMovieRentals);
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }

    [HttpGet("GetStatementLegacy")]
    public IActionResult GetStatementLegacy()
    {
       var result = _movieRentalCalculationService.GetStatementLegacy();


        return Ok(result);
    }

    private DateTime SimulateHandInTimeFromMockData(string title, DateTime startRentalTime)
    {
        var rentalData = new Dictionary<string, int>
        {
                { "The Cell", 3 },
                { "The Tigger Movie", 3 },
                { "Plan 9 from Outer Space", 1 },
                { "8 1/2", 2 },
                { "Eraserhead", 3 }
        };

        var daysRented = rentalData.ContainsKey(title) ? rentalData[title] : throw new Exception("No mock data for this movie");

        var endRentalTime = startRentalTime.AddDays(daysRented);

        return endRentalTime;
    }


}
