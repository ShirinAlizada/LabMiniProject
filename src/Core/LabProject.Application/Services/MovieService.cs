using LabProject.Application.DTO;
using LabProject.Application.Interfaces.IRepositories;
using LabProject.Application.Interfaces.IServices;
using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movies;
        private readonly IGenreRepository _genres;
        private readonly IActorRepository _actors;
        private readonly IMovieActorRepository _movieActors;

        public MovieService(
            IMovieRepository movies,
            IGenreRepository genres,
            IActorRepository actors,
            IMovieActorRepository movieActors)
        {
            _movies = movies;
            _genres = genres;
            _actors = actors;
            _movieActors = movieActors;
        }

        // 3. Create Movie
        public void Create(Movie movie)
        {
            if (movie is null)
                throw new ArgumentNullException(nameof(movie), "Movie məlumatları boş ola bilməz.");

            if (string.IsNullOrWhiteSpace(movie.Title))
                throw new ArgumentException("Title boş ola bilməz!", nameof(movie.Title));

            if (movie.ReleaseYear > DateTime.Now.Year)
                throw new ArgumentException("ReleaseYear cari ildən böyük ola bilməz!", nameof(movie.ReleaseYear));

            if (movie.Duration <= 0)
                throw new ArgumentException("Duration 0-dan böyük olmalıdır!", nameof(movie.Duration));

            if (movie.Budget <= 0)
                throw new ArgumentException("Budget 0-dan böyük olmalıdır!", nameof(movie.Budget));

            // Biznes Qaydası: Genre mövcud olmalıdır
            var genre = _genres.GetById(movie.GenreId);
            if (genre is null)
                throw new InvalidOperationException("Daxil etdiyiniz ID-yə sahib Genre tapılmadı!");

            movie.Title = movie.Title.Trim();
            movie.IsDeleted = false;

            _movies.Create(movie);
        }

        // 6. Show All Movies (silinmişlər xaric)
        public List<Movie> GetAll()
        {
            return _movies.GetAll(includeDeleted: false);
        }

        // 7. Show Movie Details
        public Movie? GetById(int id)
        {
            return _movies.GetById(id, includeDeleted: false);
        }

        // 10. Search Movie
        public List<Movie> Search(string keyword)
        {
            var list = _movies.Search(keyword ?? string.Empty);

            // Sıralama qaydası: ReleaseYear DESC, sonra Title ASC
            return list
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToList();
        }

        // 8. Assign Actor To Movie
        public void AssignActorToMovie(int movieId, int actorId)
        {
            var movie = _movies.GetById(movieId);
            if (movie is null)
                throw new InvalidOperationException("Movie tapılmadı.");

            var actor = _actors.GetById(actorId);
            if (actor is null)
                throw new InvalidOperationException("Actor tapılmadı.");

            // Biznes Qaydası: Eyni actor eyni movie-yə ikinci dəfə əlavə edilə bilməz
            if (_movieActors.Exists(movieId, actorId))
                throw new InvalidOperationException("Bu actor artıq bu filmə əlavə edilib!");

            _movieActors.Create(new MovieActor { MovieId = movieId, ActorId = actorId });
        }

        // 9. Show Movie Actors
        public List<Actor> GetMovieActors(int movieId)
        {
            var movie = _movies.GetById(movieId);
            if (movie is null)
                throw new InvalidOperationException("Movie tapılmadı.");

            return _movieActors.GetByMovieId(movieId)
                                .Select(ma => ma.Actor)
                                .Where(a => a != null)
                                .ToList()!;
        }

        // 12. Delete Movie (Soft Delete)
        public void SoftDelete(int movieId)
        {
            var movie = _movies.GetById(movieId, includeDeleted: false);
            if (movie is null)
                throw new InvalidOperationException("Movie tapılmadı (və ya artıq silinib).");

            movie.IsDeleted = true;
            _movies.Update(movie);
        }

        // 13. Restore Movie
        public void Restore(int movieId)
        {
            var movie = _movies.GetById(movieId, includeDeleted: true);
            if (movie is null)
                throw new InvalidOperationException("Movie tapılmadı.");

            if (!movie.IsDeleted)
                throw new InvalidOperationException("Bu movie artıq aktivdir, restore lazım deyil.");

            movie.IsDeleted = false;
            _movies.Update(movie);
        }

        // 11. Movie Statistics
        public MovieStatisticsDto GetStatistics()
        {
            var movies = _movies.GetAll(includeDeleted: false);
            var genres = _genres.GetAll();
            var actors = _actors.GetAll();

            var dto = new MovieStatisticsDto
            {
                TotalMovies = movies.Count,
                TotalGenres = genres.Count,
                TotalActors = actors.Count,
                AverageBudget = movies.Count > 0 ? movies.Average(m => m.Budget) : 0
            };

            var longest = movies.OrderByDescending(m => m.Duration).FirstOrDefault();
            if (longest != null)
            {
                dto.LongestMovieTitle = longest.Title;
                dto.LongestMovieDuration = longest.Duration;
            }

            // Genre üzrə Movie sayı
            dto.MovieCountByGenre = movies
                .Where(m => m.Genre != null)
                .GroupBy(m => m.Genre!.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            // Ən çox aktyoru olan Movie
            var mostActorsMovie = movies
                .OrderByDescending(m => m.MovieActors.Count)
                .FirstOrDefault();
            if (mostActorsMovie != null)
            {
                dto.MostActorsMovieTitle = mostActorsMovie.Title;
                dto.MostActorsCount = mostActorsMovie.MovieActors.Count;
            }

            // Ən çox filmdə rol alan Actor
            var mostActiveActor = actors
                .OrderByDescending(a => a.MovieActors.Count)
                .FirstOrDefault();
            if (mostActiveActor != null)
            {
                dto.MostActiveActorFullName = $"{mostActiveActor.Name} {mostActiveActor.Surname}";
                dto.MostActiveActorMovieCount = mostActiveActor.MovieActors.Count;
            }

            return dto;
        }
    }
}
