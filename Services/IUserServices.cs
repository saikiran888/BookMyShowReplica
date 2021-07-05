using System.Collections.Generic;
using BookMyShowReplica4.Models;

namespace BookMyShowReplica4.Services
{
	public interface IUserServices
	{
		public void AddMovieToUserBag(int movieId, string userId);
		public List<UserBag> GetBags(string userId);
	}
}

