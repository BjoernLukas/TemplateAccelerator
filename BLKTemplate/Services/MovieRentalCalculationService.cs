using BetaMaxRMS.BetaMaxModels;
using System.Collections;

namespace BetaMaxRMS.Services
{
    public class MovieRentalCalculationService : IMovieRentalCalculationService
    {
        //This is a recreation of the legacy code.
        //Remark: Some of variables names have been changed from legacy to make it more readable
        [Obsolete("Only use for development")]
        public string GetStatementLegacy(string customerName, IList<MovieRental> movieRentals)
        {

            ////original code
            //double totalAmount = 0;
            //int frequentRenterPoints = 0;
            //String result = "Rental Record for " + customerName + "\n";

            //for (int i = 0; i < movieRentals.Count; i++)
            //{
            //    double thisAmount = 0;
            //    var each = (MovieRental)movieRentals[i];

            //    // determines the amount for each line
            //    switch (each.Movie.PriceCode)
            //    {
            //        case Movie.REGULAR:
            //            thisAmount += 2;
            //            if (each.DaysRented > 2)
            //                thisAmount += (each.DaysRented - 2) * 1.5;
            //            break;

            //        case Movie.NEW_RELEASE:
            //            thisAmount += each.DaysRented * 3;
            //            break;

            //        case Movie.CHILDRENS:
            //            thisAmount += 1.5;
            //            if (each.DaysRented > 3)
            //                thisAmount += (each.DaysRented - 3) * 1.5;
            //            break;
            //    }

            //    frequentRenterPoints++;

            //    if (each.Movie.PriceCode == Movie.NEW_RELEASE
            //            && each.DaysRented > 1)
            //        frequentRenterPoints++;

            //    result += "\t" + each.Movie.Title + "\t"
            //        + thisAmount + "\n";
            //    totalAmount += thisAmount;

            //}

            //result += "You owed " + totalAmount + "\n";
            //result += "You earned " + frequentRenterPoints + " frequent renter points\n";

            //return result;

            throw new NotImplementedException();
        }


        public void GetTotalAmount()
        {
            throw new NotImplementedException();
        }

        public void GetFrequentRenterPoints()
        {
            throw new NotImplementedException();
        }

        //Remark: discuss the team, if this is needed and if this should be a part of MovieRentalCalculationService
        public string CreatePrettyPrint()
        {
            throw new NotImplementedException();
        }

    }
}
