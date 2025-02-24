using BLKTemplate.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BLKTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        
        [HttpGet(Name = "GetPerson")]
        public IActionResult GetPerson(string name)
        {

      

            //TODO: in time get by name via DbContext when database is done
            var newTestPerson = new Person
            { Name = "BLk", Age = 38, Description = "Do this later", Id = Guid.NewGuid() };

            if (name != newTestPerson.Name)
            { throw new Exception("Not correct name"); }

            return newTestPerson;
        }



    }
}
