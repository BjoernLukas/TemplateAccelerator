using BetaMaxRMS.BetaMaxModels;

namespace BetaMaxRMS.Services
{
    public interface IMovieRentalCalculationService
    {
        //This is a recreation of the legacy code.
        [Obsolete("Only use for local development")]
        string GetStatementLegacy(string customerName, IList<MovieRental> rentals);
    }
}