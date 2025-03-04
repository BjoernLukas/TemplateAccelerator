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

    [HttpPost("2_CreateDemoMovies")]
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

    [HttpPost("1_CreateDemoCustomer")]
    public IActionResult CreateDemoCustomer()
    {
        var customer = new Customer
        {
            Id = Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"),
            Name = "John Developer",
            Remarks = "Frequent renter",
            Gender = GenderInfo.Male
        };

        _betaMaxDbContext.Set<Customer>().Add(customer);
        _betaMaxDbContext.SaveChanges();

        return Ok(customer);
    }

    [HttpPost("3_CreateDemoRentalsForAllMovies")]
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

            //Add new price 2.0 iteration info to each movieRental
            //Todo: for next iteration this should be solved more elegantly
            switch (movie.PriceCode)
            {
                case PriceCode.Regular:
                    movieRental.BasePriceAmount = 2;
                    movieRental.NumberOfZeroCostDays = 2;
                    movieRental.PriceAmountPerDay = 1.5m;
                    break;
                case PriceCode.NewRelease:
                    movieRental.BasePriceAmount = 0;
                    movieRental.NumberOfZeroCostDays = 0;
                    movieRental.PriceAmountPerDay = 3;
                    break;
                case PriceCode.Childrens:
                    movieRental.BasePriceAmount = 1.5m;
                    movieRental.NumberOfZeroCostDays = 3;
                    movieRental.PriceAmountPerDay = 1.5m;
                    break;
            }

            allMovieRentals.Add(movieRental);
        }

        //Final step
        _betaMaxDbContext.AddRange(allMovieRentals);       
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }

    [Obsolete("OldCode")]
    [HttpGet("GetStatement")]
    public IActionResult GetStatement()
    {
        var result = _movieRentalCalculationService.GetStatementLegacy();

        return Ok(result);
    }

    [Obsolete("OldCode")]
    [HttpGet("GetTotalAmountV1")]
    public IActionResult GetTotalAmountV1()
    {
        //Id for John Developer
        var result = _movieRentalCalculationService.GetTotalAmountForCustomerV1(Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"));


        return Ok(result);
    }
    
    [HttpGet("GetTotalAmountV2")]
    public IActionResult GetTotalAmountV2()
    {
        //Id for John Developer
        var result = _movieRentalCalculationService.GetTotalAmountForCustomerV2(Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"));


        return Ok(result);
    }

    [HttpGet("GetFrequentRenterPoints")]
    public IActionResult GetFrequentRenterPoints()
    {
        var frequentRenterPoints = _movieRentalCalculationService.GetFrequentRenterPoints(Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"));

        return Ok(frequentRenterPoints);
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
