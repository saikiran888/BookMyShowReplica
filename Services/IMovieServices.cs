using System.Collections.Generic;
using BookMyShowReplica4.Models;

namespace BookMyShowReplica4.Services
{
	public interface IMovieServices
	{
		public IEnumerable<Movie> GetAllMovies();
		public Movie GetMovieDetails(int movieId);
	}
}
