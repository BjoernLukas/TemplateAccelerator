namespace BetaMaxRMS.Services
{
    public interface IMovieRentalCalculationService
    {
        //This is a recreation of the legacy code.
        [Obsolete("Legacy. Only use for local development")]
        string GetStatement();
    }
}