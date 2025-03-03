using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.AspNetCore.Mvc;
namespace BetaMaxRMS.Controllers.Api;


[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{

    private readonly BetaMaxDbContext _betaMaxDbContext;
    public MovieController(BetaMaxDbContext betaMaxDbContext)
    {
        _betaMaxDbContext = betaMaxDbContext;
    }    

    [HttpGet("GetAll")]
    public IActionResult GetAllMovies()
    {
        var result = _betaMaxDbContext.Movie.ToList();

        return Ok(result);
    }   

    [HttpPost]
    public IActionResult CreateMovie([FromBody] Movie movie)
    {
        _betaMaxDbContext.Set<Movie>().Add(movie);
        _betaMaxDbContext.SaveChanges();

        return Ok();
    }



}
