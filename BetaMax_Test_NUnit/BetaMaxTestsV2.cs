using BetaMaxRMS.BetaMaxModels;
using BetaMaxRMS.DataUtility;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace BetaMaxRMS.Tests
{
    public class BetaMaxTestsV2
    {
        private BetaMaxDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BetaMaxDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new BetaMaxDbContext(options);

            // Seed test data Customer
            _dbContext.Customer.Add(new Customer
            {
                Id = Guid.Parse("265f9212-67e1-4dda-b601-0be5b0164c06"),
                Name = "John Developer",
                Remarks = "Frequent renter",
                Gender = GenderInfo.Male
            });

            //TODO: Seed test data Movie
            //TODO: Seed test data MovieRental
            //TODO: test the MovieRentalCalculationService

            _dbContext.SaveChanges();
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
    }
}
