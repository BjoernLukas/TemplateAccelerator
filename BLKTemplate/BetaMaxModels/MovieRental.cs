using System.Data;

namespace BetaMaxRMS.BetaMaxModels
{
    /// <summary>
    /// Represents a rental agreement between a customer and BetaMax for a movie
    /// A rental agreement is created when a customer rents a movie
    /// A new rental agreement will always start with a status of Rented
    /// </summary>
    public class MovieRental
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        //With this design you can delete a movie and still have a record of the rental
        public required Guid MovieRelation { get; set; }

        public  DateTime Start { get; init; } = DateTime.Now;

        //Todo: add test for this
        //Remark for next iteration rental period should be dynamic not hardcoded
        //DueDate will always be at 23:59:59.9999999 on the seventh day after the rental starts.
        public required DateTime DueDate { get; set; } = DateTime.Now.AddDays(7).Date.AddDays(1).AddTicks(-1);

        public DateTime? HandIn { get; set; }

        public RentalStatus Status { get; set; } = RentalStatus.Rented;

        public int? DaysRented { get; set; } //Todo: decide if this should be here or in the service
    }
    public enum RentalStatus
    {
        Rented,
        Overdue,
        ReturnedOnTime,
        ReturnedOverdue,
        LostOrDamaged
    }
}