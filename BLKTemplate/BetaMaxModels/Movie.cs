namespace BetaMaxRMS.BetaMaxModels
{
    public class Movie
    {
        public required Guid Id { get; set; }

        public required string Title { get; set; }  //Improvement: Title's can vary based on countries, make TitleInfo Class 




    }
}
