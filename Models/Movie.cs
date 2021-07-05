using System;
using System.ComponentModel.DataAnnotations;

namespace BookMyShowReplica4.Models
{
	public class Movie
	{
		public int MovieId { get; set; }
		public String MovieName { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ReleaseDate { get; set; }
		public String Language { get; set; }
		public String Genre { get; set; }
		public String Price { get; set; }
	}
}

