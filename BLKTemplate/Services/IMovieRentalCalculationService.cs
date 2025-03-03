using BetaMaxRMS.BetaMaxModels;

namespace BetaMaxRMS.Services
{
    public interface IMovieRentalCalculationService
    {
        [Obsolete("Only use for development")]
        public string GetStatementLegacy();

        public decimal GetTotalAmountForCustomer(Guid customerId);


        public Movie GetMovieByRentalId(Guid MovieRelationId);


    }
}