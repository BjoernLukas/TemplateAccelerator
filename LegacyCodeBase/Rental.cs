using System;

namespace VideoStore
{
	public class Rental
	{
		private Movie movie;
		private int daysRented;

		public Rental(Movie movie, int daysRented)
		{
			this.movie = movie;
			this.daysRented = daysRented;
		}

		public int DaysRented 
		{
			get { return daysRented; }
		}

		public Movie Movie 
		{
			get { return movie; }
		}
	}
}
