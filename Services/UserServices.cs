using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using PetaPoco;
using BookMyShowReplica4.Models;
using SimpleInjector;
using System.Collections.Generic;

namespace BookMyShowReplica4.Services
{
	public class UserServices : IUserServices
	{
		private readonly Database context;
		public UserServices(Container container)
		{
			context = container.GetInstance<Database>();
		}
		public void AddMovieToUserBag(int movieId, string userId)
		{
			if (context.Fetch<UserBag>("select * from dbo.UserBags where UserId=@0 and MovieId=@1", userId, movieId).Count != 0)
			{
				var bag = context.SingleOrDefault<UserBag>("select * from dbo.UserBags where UserId=@0 and MovieId=@1", userId, movieId);
				bag.Count++;
				context.Save("dbo.UserBags", "UserBagId", bag);
			}
			else
			{
				UserBag bag = new UserBag
				{
					UserId = userId,
					MovieId = movieId,
					Count = 1
				};
				context.Insert("dbo.UserBags", bag);
			}
		}
		public List<UserBag> GetBags(string userId)
		{
			var bags = context.Fetch<UserBag>("select * from dbo.UserBags where UserId=@0", userId);
			if (bags == null)
			{
				return null;
			}
			return bags;
		}
	}
}
