namespace BLKTemplate.Models
{
    public class Person
    {
        public Guid Id { get; set; }    

        public required string Name { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
  


    }
}
