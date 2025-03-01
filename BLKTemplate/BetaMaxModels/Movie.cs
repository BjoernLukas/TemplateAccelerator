namespace BetaMaxRMS.BetaMaxModels
{
    public class Movie
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public required string Title { get; set; }  //Improvement: Title's can vary based on countries, make TitleInfo Class?

        public required PriceCode PriceCode { get; set; }                

    }
}
