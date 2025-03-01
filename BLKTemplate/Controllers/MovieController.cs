using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.AspNetCore.Mvc;
namespace BetaMaxRMS.Controllers;


[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{

    private readonly BetaMaxDbContext _betaMaxDbContext;
    public MovieController(BetaMaxDbContext betaMaxDbContext)
    {
        _betaMaxDbContext = betaMaxDbContext;
    }
    

    [HttpGet("ByTitle/{searchTitle}")]
    public IActionResult GetMovieByTitle(string searchTitle)
    {
        var movie = _betaMaxDbContext.Set<Movie>().Where(p => p.Title == searchTitle).FirstOrDefault();
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpGet("GetAll")]
    public IActionResult GetAllMovies()
    {
        var result = _betaMaxDbContext.Movie.ToList();

        return Ok(result);
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

    [HttpPost]
    public IActionResult CreateMovie([FromBody] Movie movie)
    {
        _betaMaxDbContext.Set<Movie>().Add(movie);
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }



}
