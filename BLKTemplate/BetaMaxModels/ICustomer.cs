namespace BetaMaxRMS.BetaMaxModels
{
    public interface ICustomer
    {
        string Name { get; set; }
        IList<RentalRecord> Rentals { get; set; }

        void AddRental(RentalRecord rental);
        string GetStatement();
    }
}