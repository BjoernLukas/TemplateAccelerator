namespace TemplateAcceleratorV1.Models
{
    public interface IPersonRepository
    {
        Person GetPerson(Guid PersonId);
        IEnumerable<Person> GetAllPersons();
    }
}
