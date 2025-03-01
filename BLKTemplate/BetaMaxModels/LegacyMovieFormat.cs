namespace BetaMaxRMS.BetaMaxModels
{

    /// <summary>
    /// This is a wrapper class for the use of the GetStatement legacy code.
    /// </summary>
    public class LegacyMovieFormat
    {
       
        public const int CHILDRENS = 2;
        public const int REGULAR = 0;
        public const int NEW_RELEASE = 1;

        public required Movie Movie { get; set; }

        

    }
}
