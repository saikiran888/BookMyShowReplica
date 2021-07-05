using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using BookMyShowReplica4.Models;
using PetaPoco;
using SimpleInjector;
using System.Collections.Generic;


namespace BookMyShowReplica4.Services
{
	public class MovieServices : IMovieServices
	{
		private readonly Database context;
		public MovieServices(Container container)
		{
			context = container.GetInstance<Database>();
		}
		public IEnumerable<Movie> GetAllMovies()
		{
			var movies = context.Query<Movie>("Select * from dbo.Movies");
			return movies;
		}
		public Movie GetMovieDetails(int movieId)
		{
			var movie = context.SingleOrDefault<Movie>("select * from dbo.Movies where MovieId=@0", movieId);
			return movie;
		}
	}
}
