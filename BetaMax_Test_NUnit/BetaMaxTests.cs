using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using BetaMaxRMS.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BetaMax_Test_NUnit
{
    [TestFixture]
    public class BetaMaxTests
    {
        private Customer _customer;
        private List<Movie> _Allmovies;
        private List<MovieRental> _movieRentalsForAllMovies;

        [SetUp]
        public void Setup()
        {
            //Same DemoData as in DevUtilityController

            //Customer
            _customer = new Customer
            {
                Id = Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"),
                Name = "John Developer",
                Remarks = "Frequent renter",
                Gender = GenderInfo.Male
            };

            //Movie
            _Allmovies = new List<Movie>
            {
                new() { Title = "The Cell", PriceCode = PriceCode.NewRelease },
                new() { Title = "The Tigger Movie", PriceCode = PriceCode.Childrens },
                new() { Title = "Plan 9 from Outer Space", PriceCode = PriceCode.Regular },
                new() { Title = "8 1/2", PriceCode = PriceCode.Regular },
                new() { Title = "Eraserhead", PriceCode = PriceCode.Regular }
            };

            //MovieRentals
            //Remark: sure there is a better way of doing this!!
            //problem is my setup is wrong all test could be affected by this
            //mockRepository like in XUnit? Well Make it work, make it right....            
            _movieRentalsForAllMovies = CreateDemoRentalsMovies(_customer,_Allmovies);

        }

        [Test]
        public void TestCustomerRemarks()
        {
            // Arrange
            string expectedRemarks = "Frequent renter";

            // Act
            string actualRemarks = _customer.Remarks;

            // Assert
            Assert.That(actualRemarks, Is.EqualTo(expectedRemarks));
        }

        [Test]
        public void TestGetFrequentRenterPoints()
        {


        }

        [Test]
        public void SingleNewReleaseStatement()
        {
            // Arrange
            var singleMovie = new Movie { Title = "The Cell", PriceCode = PriceCode.NewRelease };
            var singleMovieRental = CreateDemoRentalsMovies(_customer, new List<Movie> { singleMovie }).First();

            var movieRentalCalculationService = new MovieRentalCalculationService();

            //Will not work before I get Moq working with a MockRepository
            //movieRentalCalculationService.GetFrequentRenterPoints(singleMovieRental);
        }

        /// <summary>
        /// Recursive problem how are we going to unit test this?
        /// </summary>        
        /// <returns></returns>
        private static List<MovieRental> CreateDemoRentalsMovies(Customer customer, List<Movie> movies)
        {
          
            var startRentalTime = new DateTime(2025, 3, 1, 20, 0, 0);

            var allMovieRentals = new List<MovieRental>();
            //Create a movieRental for each movie
            foreach (var movie in movies)
            {
                var movieRental = new MovieRental
                {
                    MovieRelation = movie.Id,
                    CustomerRelation = customer.Id,
                    Start = startRentalTime
                };

                var handIndTime = SimulateHandInTimeForMockData(movie.Title, startRentalTime);
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

            return (allMovieRentals);
        }

        private static DateTime SimulateHandInTimeForMockData(string title, DateTime startRentalTime)
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
    }
}
