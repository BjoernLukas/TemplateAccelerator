namespace TemplateAcceleratorV1.Models
{
    public class PersonRepository
    {
        private readonly TemplateDbContext _dbContext;

        public PersonRepository(TemplateDbContext acceleratorDbContext)
        {
            _dbContext = acceleratorDbContext ?? throw new ArgumentNullException(nameof(acceleratorDbContext));
        }

        // https://app.pluralsight.com/ilx/video-courses/72ba6cdd-6f01-4bf1-a17a-37419596f317/8ba3f52b-db88-4611-b414-b2ba90d0151b/cfe35cd6-4fcc-4923-b911-0589e9381688


       
    }
}
