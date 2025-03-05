using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using BetaMaxRMS.Services;
using Microsoft.EntityFrameworkCore;


namespace BetaMaxRMS.Tests
{
    public class BetaMaxTestsV2
    {
        private BetaMaxDbContext _dbContext;
        private MovieRentalCalculationService _rentalCalculationService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BetaMaxDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new BetaMaxDbContext(options);

            _dbContext.Customer.Add(CreateNewCustomer());
            _dbContext.Movie.AddRange(CreateAllMovies());            
            _dbContext.SaveChanges();
            //MovieRental will be added in the test methods depending on the test case

            _rentalCalculationService = new MovieRentalCalculationService(_dbContext);
        }


        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose(); // Dispose the DbContext after each test
        }

        [Test]
        public void TestCustomer()
        {
            // Arrange
            var expectedName = "John Developer";

            // Act
            var customer = _dbContext.Customer.SingleOrDefault();

            // Assert
            Assert.That(customer.Name, Is.EqualTo(expectedName));

        }

        [Test]
        public void FrequentRenterPointsSingleNewRelease()
        {
            // Arrange            
            var expectedPoints = 2; //The cell is a new release 1 point + 1 bonus point = 2
            var customer = _dbContext.Customer.Single();
            var specificMovies = _dbContext.Movie.Where(m => m.Title == "The Cell").ToList();
            var movieRentals = CreateDemoRentalsForSpecificMovies(specificMovies);

            _dbContext.MovieRental.AddRange(movieRentals);
            _dbContext.SaveChanges();

            // Act
            var frequentRenterPoints = _rentalCalculationService.GetFrequentRenterPoints(customer.Id);

            // Assert
            Assert.That(frequentRenterPoints, Is.EqualTo(expectedPoints));

        }

        [Test]
        public void GetTotalAmountSingleNewRelease()
        {
            // Arrange
            var expectedAmount = 9; //The cell is a new release 3 * 3 = 9
            var customer = _dbContext.Customer.Single();
            var specificMovies = _dbContext.Movie.Where(m => m.Title == "The Cell").ToList();
            var movieRentals = CreateDemoRentalsForSpecificMovies(specificMovies);

            _dbContext.MovieRental.AddRange(movieRentals);
            _dbContext.SaveChanges();

            // Act
            var calculatedAmount = _rentalCalculationService.GetTotalAmountForCustomerV2(customer.Id);

            // Assert
            Assert.That(calculatedAmount, Is.EqualTo(expectedAmount));

        }

        [Test]
        public void GetTotalAmountMultipleRegular()
        {
            // Arrange
            var expectedAmount = 7.5m;
            var customer = _dbContext.Customer.Single();
            var movieTitles = new List<string> { "Plan 9 from Outer Space", "8 1/2", "Eraserhead" };
            var specificMovies = _dbContext.Movie.Where(m => movieTitles.Contains(m.Title)).ToList();
            var movieRentals = CreateDemoRentalsForSpecificMovies(specificMovies);

            _dbContext.MovieRental.AddRange(movieRentals);
            _dbContext.SaveChanges();

            // Act
            var calculatedAmount = _rentalCalculationService.GetTotalAmountForCustomerV2(customer.Id);

            // Assert
            Assert.That(calculatedAmount, Is.EqualTo(expectedAmount));

        }

        //TODO: Add Unit Test for SimulateHandInTimeMockData and UpdateWhenHandIn


        #region Helper Methods

        private static List<Movie> CreateAllMovies()
        {
            return new List<Movie>
            {
            new() { Title = "The Cell", PriceCode = PriceCode.NewRelease },
            new() { Title = "The Tigger Movie", PriceCode = PriceCode.Childrens },
            new() { Title = "Plan 9 from Outer Space", PriceCode = PriceCode.Regular },
            new() { Title = "8 1/2", PriceCode = PriceCode.Regular },
            new() { Title = "Eraserhead", PriceCode = PriceCode.Regular }
            };
        }

        private static Customer CreateNewCustomer()
        {
            return new Customer
            {
                Id = Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"),
                Name = "John Developer",
                Remarks = "Frequent renter",
                Gender = GenderInfo.Male
            };
        }

        private List<MovieRental> CreateDemoRentalsForSpecificMovies(List<Movie> movies)
        {
            var customer = _dbContext.Customer.Single();
            var startRentalTime = new DateTime(2025, 3, 1, 20, 0, 0);
            var allMovieRentals = new List<MovieRental>();

            foreach (var movie in movies)
            {
                var movieRental = new MovieRental
                {
                    MovieRelation = movie.Id,
                    CustomerRelation = customer.Id,
                    Start = startRentalTime
                };

                var handIndTime = SimulateHandInTimeMockData(movie.Title, startRentalTime);
                movieRental.UpdateWhenHandIn(handIndTime);

                //Add new price 2.0 iteration info to each movieRental
                //Todo: for next iteration this should be solved more elegantly
                switch (movie.PriceCode)
                {
                    case PriceCode.Regular:
                        movieRental.BasePriceAmount = 2;
                        movieRental.NumberOfZeroCostDays = 2;
                        movieRental.PriceAmountPerDay = 1.5m;
                        break;
                    case PriceCode.NewRelease:
                        movieRental.BasePriceAmount = 0;
                        movieRental.NumberOfZeroCostDays = 0;
                        movieRental.PriceAmountPerDay = 3;
                        break;
                    case PriceCode.Childrens:
                        movieRental.BasePriceAmount = 1.5m;
                        movieRental.NumberOfZeroCostDays = 3;
                        movieRental.PriceAmountPerDay = 1.5m;
                        break;
                }
                allMovieRentals.Add(movieRental);
            }
            return allMovieRentals;
        }

        //This is a code smell. This method should be refactored to a utility class, I am violating DRY!
        private static DateTime SimulateHandInTimeMockData(string title, DateTime startRentalTime)
        {
            var rentalData = new Dictionary<string, int>
        {
                { "The Cell", 3 },
                { "The Tigger Movie", 3 },
                { "Plan 9 from Outer Space", 1 },
                { "8 1/2", 2 },
                { "Eraserhead", 3 }
        };

            var daysRented = rentalData.ContainsKey(title) ? rentalData[title] : throw new Exception("No mock data for this movie");

            var endRentalTime = startRentalTime.AddDays(daysRented);

            return endRentalTime;
        }       

        #endregion
    }


}
