using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BookMyShowReplica4.Services;
using BookMyShowReplica4.Models;
using System.Security.Claims;
using SimpleInjector;
using Microsoft.AspNetCore.Http;

namespace BookMyShowReplica4.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MovieController : ControllerBase
	{
		private readonly IMovieServices movieServices;
		public MovieController(Container container)
		{
			this.movieServices = container.GetInstance<IMovieServices>();
		}
		[HttpGet("movies")]
		public List<Movie> Get()
		{
			var movies = new List<Movie>();
			var bags = this.movieServices.GetAllMovies();
			foreach (var movie in bags)
			{
				movies.Add(movie);
			}
			if (movies.Count == 0)
			{
				return null;
			}
			return movies;
		}
		[HttpGet("movies/id={id:int}")]
		public Movie GetMovie(int id)
		{
			var movie = this.movieServices.GetMovieDetails(id);
			return movie;
		}
	}
}


