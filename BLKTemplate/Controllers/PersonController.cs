using Microsoft.AspNetCore.Mvc;
using TemplateAcceleratorV1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TemplateAcceleratorV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {


        [HttpGet(Name = "GetPerson")]
        public IActionResult GetPerson(string name)
        {

            //TODO: in time get by name via TemplateDbContext when database is done
            var newTestPerson = new Person
            { Name = "BLK", Age = 38, Description = "Do this later", PersonId = Guid.NewGuid() };

            if (name != newTestPerson.Name)
            { throw new Exception("Incorrect input name"); }



            return Ok(newTestPerson); //TODO: add something like this return result is null ? NotFound() : Ok(result);
        }



    }
}
