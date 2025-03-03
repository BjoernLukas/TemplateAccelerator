using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
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
            var customerName = "Joe";
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


                //original Comment: determines the amount for currentMovieRental line
                //Change the switch, but it still switches on each eachMovieRentals priceCode  
                switch (currentMovie.PriceCode)
                {
                    case PriceCode.Regular:
                        thisAmount += 2;
                        if (currentMovieRental.DaysRented > 2)
                            thisAmount += (double)((currentMovieRental.DaysRented - 2) * 1.5); //WARNING: check if precision is lost when casting.
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

                result += "\t" + currentMovie.Title + "\t"
                    + thisAmount + "\n";
                totalAmount += thisAmount;

            }

            result += "You owed " + totalAmount + "\n";
            result += "You earned " + frequentRenterPoints + " frequent renter points\n";

            return result;
        }

        //Discussion: Should this be part of a repository service?
        private Movie GetMovieByRentalId(Guid RentalId)
        {
            var MovieRelationId = (_betaMaxDbContext.Set<MovieRental>().SingleOrDefault(rental => rental.Id == RentalId)?.MovieRelation)
                ?? throw new Exception("No MovieRental found");

            var movie = _betaMaxDbContext.Set<Movie>().SingleOrDefault(p => p.Id == MovieRelationId)
                ?? throw new Exception("No movie found");

            return movie;
        }


        public void GetTotalAmount()
        {


            //Change from double to decimal. 
            //Double is best for artefacts of nature which can't really be measured exactly.
            decimal totalAmount = 0;




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
