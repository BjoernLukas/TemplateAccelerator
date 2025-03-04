using BetaMaxRMS.BetaMaxModels;

namespace BetaMaxRMS.Services
{
    public interface IMovieRentalCalculationService
    {
        [Obsolete("Only use for development")]
        public string GetStatementLegacy();

        [Obsolete("Use v2")]
        public decimal GetTotalAmountForCustomerV1(Guid customerId);

        public decimal GetTotalAmountForCustomerV2(Guid customerId);

        public int GetFrequentRenterPoints(Guid customerId);


    }
}