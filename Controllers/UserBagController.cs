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
using PetaPoco;
using Microsoft.AspNetCore.Http;

namespace BookMyShowReplica4.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class UserBagController : ControllerBase
	{
		private string currentUserId;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IUserServices userServices;
		private readonly IMovieServices movieServices;
		public UserBagController(Container container)
		{
			httpContextAccessor = container.GetInstance<IHttpContextAccessor>();

			movieServices = container.GetInstance<IMovieServices>();
			userServices = container.GetInstance<IUserServices>();

		}
		[HttpGet("usermovies")]
		public List<Movie> GetUserMovies()
		{
			var movies = new List<Movie>();
			currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var bags = userServices.GetBags(currentUserId);
			foreach (var bag in bags)
			{
				var movie = movieServices.GetMovieDetails(bag.MovieId);
				movies.Add(movie);
			}
			if (movies.Count == 0)
			{
				return null;
			}
			return movies;
		}
		[HttpPost("usermovies/{id:int}")]
		public IActionResult PostMovie(int id)
		{
			currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			userServices.AddMovieToUserBag(id, currentUserId);
			return Ok();
		}
	}
}
