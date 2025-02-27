using System;

namespace VideoStore
{
	using System.Collections;

	public class Customer
	{
		private string name;
		private IList rentals = new ArrayList();

		public Customer(string name)
		{
			this.name = name;
		}

		public void AddRental(Rental rental) 
		{
			rentals.Add(rental);
		}

		public string GetStatement() 
		{
			double 				totalAmount 			= 0;
			int					frequentRenterPoints 	= 0;
			String 				result 					= "Rental Record for " + name + "\n";
		
			for(int i = 0; i < rentals.Count; i++ ){
				double 		thisAmount = 0;
                Rental each = (Rental)rentals[i];
			    
				// determines the amount for each line
				switch (each.Movie.PriceCode) 
				{
					case Movie.REGULAR:
						thisAmount += 2;
						if (each.DaysRented > 2)
							thisAmount += (each.DaysRented - 2) * 1.5;
						break;
					case Movie.NEW_RELEASE:
						thisAmount += each.DaysRented * 3;
						break;
					case Movie.CHILDRENS:
						thisAmount += 1.5;
						if (each.DaysRented > 3)
							thisAmount += (each.DaysRented - 3) * 1.5;
						break;
				}
			
				frequentRenterPoints++;
			
				if (each.Movie.PriceCode == Movie.NEW_RELEASE 
						&& each.DaysRented > 1)
					frequentRenterPoints++;
				
				result += "\t" + each.Movie.Title + "\t"
					+ thisAmount + "\n";
				totalAmount += thisAmount;
				
			}
		
			result += "You owed " + totalAmount + "\n";
			result += "You earned " + frequentRenterPoints + " frequent renter points\n";
		
			return result;
		}
	}
}
