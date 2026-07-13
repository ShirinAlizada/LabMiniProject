using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.DTO
{
    public class MovieStatisticsDto
    {
        public int TotalMovies { get; set; }
        public int TotalActors { get; set; }
        public int TotalGenres { get; set; }
        public decimal AverageBudget { get; set; }

        public string? LongestMovieTitle { get; set; }
        public decimal LongestMovieDuration { get; set; }

        public Dictionary<string, int> MovieCountByGenre { get; set; } = new();

        public string? MostActorsMovieTitle { get; set; }
        public int MostActorsCount { get; set; }

        public string? MostActiveActorFullName { get; set; }
        public int MostActiveActorMovieCount { get; set; }
    }
}
