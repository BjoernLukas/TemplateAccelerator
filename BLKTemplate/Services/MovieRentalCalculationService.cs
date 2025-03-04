using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections;

namespace BetaMaxRMS.Services
{
    public class MovieRentalCalculationService : IMovieRentalCalculationService
    {
        private readonly BetaMaxDbContext _betaMaxDbContext;

        public MovieRentalCalculationService(BetaMaxDbContext betaMaxDbContext)
        {
            _betaMaxDbContext = betaMaxDbContext;
        }


        //This is a recreation of the legacy code.
        //Remark: Some of variables names have been changed from legacy to make it more readable
        [Obsolete("Only use for development")]
        public string GetStatementLegacy()
        {
            //SetUp to run original code
            var customerName = "Fred";
            var movieRentals = _betaMaxDbContext.MovieRental.Where(p => p.DaysRented != null).ToList();




            //****As close as possible to original code, 
            double totalAmount = 0;
            int frequentRenterPoints = 0;
            String result = "Rental Record for " + customerName + "\n";

            for (int i = 0; i < movieRentals.Count; i++)
            {
                double thisAmount = 0;
                var currentMovieRental = (MovieRental)movieRentals[i];
                var currentMovie = GetMovieByRentalId(currentMovieRental.MovieRelation);

                //***For testing only create units tests for this
                //if (currentMovie.Title != "Plan 9 from Outer Space" && currentMovie.Title != "8 1/2" && currentMovie.Title != "Eraserhead")
                //{ continue; }


                //original Comment: determines the amount for rental line
                //Change the switch, but it still switches on each eachMovieRentals priceCode  
                switch (currentMovie.PriceCode)
                {
                    case PriceCode.Regular:
                        thisAmount += 2;
                        if (currentMovieRental.DaysRented > 2)
                            thisAmount += (double)((currentMovieRental.DaysRented - 2) * 1.5); //Todo: Check if precision is lost when casting.. its ok but the the other way around 
                        break;

                    case PriceCode.NewRelease:
                        thisAmount += (double)(currentMovieRental.DaysRented * 3);
                        break;

                    case PriceCode.Childrens:
                        thisAmount += 1.5;
                        if (currentMovieRental.DaysRented > 3)
                            thisAmount += (double)((currentMovieRental.DaysRented - 3) * 1.5);
                        break;
                }

                frequentRenterPoints++;

                if (currentMovie.PriceCode == PriceCode.NewRelease
                        && currentMovieRental.DaysRented > 1)
                    frequentRenterPoints++;

                result += "\t" + currentMovie.Title + "\t" + thisAmount + "\n";
                totalAmount += thisAmount;


            }

            result += "You owed " + totalAmount + "\n";
            result += "You earned " + frequentRenterPoints + " frequent renter points\n";

            return result;
        }

        [Obsolete("See v2")]
        public decimal GetTotalAmountForCustomerV1(Guid customerId)
        {

            //Get all currentMovie rentals for the customer
            var movieRentals = _betaMaxDbContext.MovieRental.Where(p => p.CustomerRelation == customerId).ToList();

            //Change from double to decimal. -- Double is best for artefacts of nature which can't really be measured exactly.
            var totalPriceAmount = 0m;

            foreach (var currentMovieRental in movieRentals)
            {
                var currentMovie = GetMovieByRentalId(currentMovieRental.MovieRelation);
                var basePriceAmount = 0m;

                switch (currentMovie.PriceCode)
                {
                    case PriceCode.Regular:
                        basePriceAmount += 2;
                        if (currentMovieRental.DaysRented > 2)
                        {
                            basePriceAmount += (decimal)((currentMovieRental.DaysRented - 2) * 1.5);
                        }
                        break;
                    case PriceCode.NewRelease:
                        basePriceAmount += (decimal)(currentMovieRental.DaysRented * 3);
                        break;
                    case PriceCode.Childrens:
                        basePriceAmount += (decimal)1.5;
                        if (currentMovieRental.DaysRented > 3)
                        {
                            basePriceAmount += (decimal)((currentMovieRental.DaysRented - 3) * 1.5);
                        }
                        break;
                }

                totalPriceAmount += basePriceAmount;

            }

            return totalPriceAmount;
        }

        public decimal GetTotalAmountForCustomerV2(Guid customerId)
        {

            //Get all currentMovie rentals for the customer
            var movieRentals = _betaMaxDbContext.MovieRental.Where(x => x.CustomerRelation == customerId && x.DaysRented != null).ToList();

            //Change from double to decimal. -- Double is best for artefacts of nature which can't really be measured exactly.
            var priceAmountForAllRentals = 0m;

            foreach (var rental in movieRentals)
            {
                var currentMovie = GetMovieByRentalId(rental.MovieRelation);
                var currentPriceAmount = 0m;           
               
                var daysAboveZeroCost = rental.DaysRented - rental.NumberOfZeroCostDays;
                

                //Step 1 add price for days rented above zero-cost-days
                if (rental.DaysRented > rental.NumberOfZeroCostDays)
                { currentPriceAmount += rental.PriceAmountPerDay * daysAboveZeroCost.Value; }

                //Step 2 add base price if any
                currentPriceAmount += rental.BasePriceAmount;

                //Step 3 add to running total
                priceAmountForAllRentals += currentPriceAmount;
            }

            return priceAmountForAllRentals;
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

        //Discussion: Should this be part of a repository service?
        private Movie GetMovieByRentalId(Guid MovieRelationId)
        {

            var MovieRelation = _betaMaxDbContext.Set<MovieRental>().SingleOrDefault(rental => rental.MovieRelation == MovieRelationId)?.MovieRelation
                 ?? throw new Exception("No MovieRelation found");

            var movie = _betaMaxDbContext.Set<Movie>().SingleOrDefault(p => p.Id == MovieRelation)
                ?? throw new Exception("No currentMovie found");

            return movie;
        }
    }
}
