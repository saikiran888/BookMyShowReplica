
using JsonNet.PrivateSettersContractResolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using BookMyShowReplica4.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Data.SqlClient;
using PetaPoco;

namespace BookMyShowReplica4.Models
{
	public class Seeding
	{
		public static void Seed(string jsonData, IConfiguration configuration)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				ContractResolver = new PrivateSetterContractResolver()
			};
			List<Movie> movies =
			 JsonConvert.DeserializeObject<List<Movie>>(
			   jsonData, settings);
			var connection = new SqlConnection(configuration.GetConnectionString("ApplicationDbContextConnection"));
			connection.Open();
			using (var context = new Database(connection))
			{
				var moviesList = context.Query<Movie>("SELECT * FROM dbo.Movies").ToList<Movie>();
				if (moviesList != movies)
				{
					foreach (var movie in movies)
					{
						if (context.Fetch<Movie>("select * from dbo.Movies where MovieId=@0", movie.MovieId).Any<Movie>())
						{
							context.Save("dbo.Movies", "MovieId", movie);
						}
						else
						{
							context.Insert("dbo.Movies", movie);
						}
					}

				}
			}
			connection.Close();
		}
	}
}

