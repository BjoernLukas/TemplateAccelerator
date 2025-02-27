namespace BetaMaxRMS.BetaMaxModels
{


    public class BetaMaxCustomer : ICustomer
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Remarks { get; set; }

        public GenderInfo? Gender { get; set; }
        IList<RentalRecord> ICustomer.Rentals { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void ICustomer.AddRental(RentalRecord rental)
        {
            throw new NotImplementedException();
        }

        string ICustomer.GetStatement()
        {
            throw new NotImplementedException();
        }
    }

    public enum GenderInfo
    {
        Male,
        Female,
        Other,

    }

}
