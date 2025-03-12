using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateAcceleratorV1.Models
{

    /// <summary>
    /// Person class still work in progress
    /// </summary>
    public class Person
    {
        /// <summary>
        /// PersonId is a unique identifier for each person
        /// </summary>        
        public Guid PersonId { get; init; } = Guid.NewGuid();

        /// <summary>
        /// Name of the person including middle name and last name
        /// </summary>
        [Required]
        public required string Name { get; set; }

        /// <summary>
        /// Age of the person, in full years.
        /// </summary>       
        public int? Age { get; set; }

        /// <summary>
        /// Description of the person
        /// </summary>
        [DefaultValue("No Description yet")]
        public string? Description { get; set; } = "No Description yet";

        /// <summary>
        /// Gender Enum, including Male,Female and Other
        /// </summary>
        public GenderInfo? Gender { get; set; }
    }

    public enum GenderInfo
    {
        Male,
        Female,
        Other,

    }

}
