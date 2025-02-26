using TemplateAcceleratorV1.DataUtility;

namespace TemplateAcceleratorV1.Models
{
    public class PersonRepository : IPersonRepository
    {
        private readonly TemplateDbContext _dbContext;

        public PersonRepository(TemplateDbContext acceleratorDbContext)
        {
            _dbContext = acceleratorDbContext ?? throw new ArgumentNullException(nameof(acceleratorDbContext));
        }

        public IEnumerable<Person> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public Person GetPerson(Guid PersonId)
        {
            throw new NotImplementedException();
        }

       



    }
}
