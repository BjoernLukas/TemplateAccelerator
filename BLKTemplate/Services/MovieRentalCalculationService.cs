namespace BetaMaxRMS.Services
{
    public class MovieRentalCalculationService : IMovieRentalCalculationService
    {
        //This is a recreation of the legacy code.
        [Obsolete("Legacy. Only use for local development")]
        public string GetStatement()
        {
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

        //Remark: discuss the team, if this should be a part of MovieRentalCalculationService
        public string CreatePrettyPrint()
        {
            throw new NotImplementedException();
        }

    }
}
