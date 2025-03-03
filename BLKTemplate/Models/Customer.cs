namespace BetaMaxRMS.BetaMaxModels
{
    public class Customer
    {      
        //Remark: init could be used in a case where I want to recreate old Customers from an old system. 
        public  Guid Id { get; init; } = Guid.NewGuid();

        public required string Name { get; set; }

        public string? Remarks { get; set; }

        public GenderInfo? Gender { get; set; }

        public List<MovieRental> MovieRentals { get; init; } = [];
    }

    public enum GenderInfo
    {
        Male,
        Female,
        Other,
    }

}
