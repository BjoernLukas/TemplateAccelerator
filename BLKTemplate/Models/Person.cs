namespace TemplateAcceleratorV1.Models
{


    public class Person
    {
        public required Guid PersonId { get; set; }

        public required string Name { get; set; }

        public int? Age { get; set; }

        public string? Description { get; set; }

        public GenderInfo? Gender { get; set; }
    }

    public enum GenderInfo
    {
        Male,
        Female,
        Other,

    }

}
