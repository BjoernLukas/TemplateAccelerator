using System.Data;
using System.Reflection.Metadata;

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
        public required Guid MovieRelation { get; init; }
        public required Guid CustomerRelation { get; init; }
        public required DateTime Start { get; init; } = DateTime.Now;

        //Todo: add test for this
        //Remark for next iteration rental period should be dynamic not hardcoded at 7+ days
        //DueDate will always be at 23:59:59.9999999 on the seventh day after the rental starts.
        public DateTime DueDate { get; init; } = DateTime.Now.AddDays(8).AddTicks(-1);

        public DateTime? HandIn { get; private set; }

        public RentalStatus Status { get; private set; } = RentalStatus.Rented;

        public int? DaysRented { get; private set; } 

        public void UpdateWhenHandIn(DateTime updateTime)
        {
            DaysRented = (int)(updateTime - Start).TotalDays; //Todo: Improve this with a graze period meaning full days only


            if (updateTime > DueDate)
            {
                HandIn = updateTime;
                Status = RentalStatus.ReturnedOverdue;
            }
            else
            {
                HandIn = updateTime;
                Status = RentalStatus.ReturnedOnTime;
            }

            if (updateTime < Start)
            {
                throw new Exception("Invalid update time");
            }
        }

        //New fields for the this price 2.0 iteration
        public decimal BasePriceAmount { get; set; }

        public int NumberOfZeroCostDays { get; set; }

        public decimal PriceAmountPerDay { get; set; }

    }
    public enum RentalStatus
    {
        Rented,
        Overdue, //Todo: Talk with the team we are going to implement this. Could be on lookup only?
        ReturnedOnTime,
        ReturnedOverdue,
    }
}