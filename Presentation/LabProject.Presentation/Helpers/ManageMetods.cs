using LabProject.Application.DTO;
using LabProject.Application.Interfaces.IServices;
using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Presentation.Helpers
{
    public class ManageMetods
    {
        private readonly IGenreService _genres;
        private readonly IActorService _actors;
        private readonly IMovieService _movies;

        public ManageMetods(IGenreService genres, IActorService actors, IMovieService movies)
        {
            _genres = genres;
            _actors = actors;
            _movies = movies;
        }

        private static bool HasAtLeastOneLetter(string s) => s.Any(char.IsLetter);

        // 1. Create Genre
        public void CreateGenre()
        {
            MenuManagement.RightBegin("Create Genre");

            var name = (MenuManagement.RightAsk("Genre name (menu): ") ?? "").Trim();
            if (name.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }

            if (string.IsNullOrWhiteSpace(name) || !HasAtLeastOneLetter(name))
            {
                MenuManagement.RightError("Name must not be empty and must contain a letter.");
                MenuManagement.RightEnd(); return;
            }

            try
            {
                _genres.Create(new Genre { Name = name });
                MenuManagement.RightSuccess("Genre created.");
            }
            catch (Exception ex)
            {
                MenuManagement.RightError($"Error: {ex.Message}");
            }

            MenuManagement.RightEnd();
        }

        // 2. Create Actor
        public void CreateActor()
        {
            int step = 0;
            string name = "", surname = "", country = "";
            DateTime birthDate = default;

            while (true)
            {
                MenuManagement.RightBegin("Create Actor");
                if (!string.IsNullOrWhiteSpace(name)) MenuManagement.RightWriteLine($"Name    : {name}");
                if (!string.IsNullOrWhiteSpace(surname)) MenuManagement.RightWriteLine($"Surname : {surname}");
                if (birthDate != default) MenuManagement.RightWriteLine($"Birth   : {birthDate:yyyy-MM-dd}");
                if (!string.IsNullOrWhiteSpace(country)) MenuManagement.RightWriteLine($"Country : {country}");
                MenuManagement.RightWriteLine("");

                if (step == 0)
                {
                    var input = (MenuManagement.RightAsk("Name (menu): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (string.IsNullOrWhiteSpace(input) || !HasAtLeastOneLetter(input))
                    {
                        MenuManagement.RightError("Name must not be empty.");
                        MenuManagement.RightEnd(); continue;
                    }
                    name = input;
                    step = 1;
                }
                else if (step == 1)
                {
                    var input = (MenuManagement.RightAsk("Surname (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 0; continue; }
                    if (string.IsNullOrWhiteSpace(input) || !HasAtLeastOneLetter(input))
                    {
                        MenuManagement.RightError("Surname must not be empty.");
                        MenuManagement.RightEnd(); continue;
                    }
                    surname = input;
                    step = 2;
                }
                else if (step == 2)
                {
                    var input = (MenuManagement.RightAsk("Birth date (yyyy-MM-dd) (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 1; continue; }
                    if (!DateTime.TryParseExact(input, "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out birthDate))
                    {
                        MenuManagement.RightError("Use yyyy-MM-dd format.");
                        MenuManagement.RightEnd(); continue;
                    }
                    step = 3;
                }
                else
                {
                    var input = (MenuManagement.RightAsk("Country (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 2; continue; }
                    if (string.IsNullOrWhiteSpace(input) || !HasAtLeastOneLetter(input))
                    {
                        MenuManagement.RightError("Country must not be empty.");
                        MenuManagement.RightEnd(); continue;
                    }
                    country = input;

                    try
                    {
                        _actors.Create(new Actor { Name = name, Surname = surname, BirthDate = birthDate, Country = country });
                        MenuManagement.RightSuccess("Actor created.");
                    }
                    catch (Exception ex)
                    {
                        MenuManagement.RightError($"Error: {ex.Message}");
                    }
                    MenuManagement.RightEnd(); return;
                }

                MenuManagement.RightEnd();
            }
        }

        // 3. Create Movie
        public void CreateMovie()
        {
            int step = 0;
            string title = "";
            int releaseYear = 0, duration = 0, genreId = 0;
            decimal budget = 0;

            while (true)
            {
                MenuManagement.RightBegin("Create Movie");
                if (!string.IsNullOrWhiteSpace(title)) MenuManagement.RightWriteLine($"Title   : {title}");
                if (releaseYear > 0) MenuManagement.RightWriteLine($"Year    : {releaseYear}");
                if (duration > 0) MenuManagement.RightWriteLine($"Duration: {duration}");
                if (budget > 0) MenuManagement.RightWriteLine($"Budget  : {budget}");
                MenuManagement.RightWriteLine("");

                if (step == 0)
                {
                    var input = (MenuManagement.RightAsk("Title (menu): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        MenuManagement.RightError("Title must not be empty.");
                        MenuManagement.RightEnd(); continue;
                    }
                    title = input;
                    step = 1;
                }
                else if (step == 1)
                {
                    var input = (MenuManagement.RightAsk("Release year (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 0; continue; }
                    if (!int.TryParse(input, out releaseYear) || releaseYear > DateTime.Now.Year || releaseYear <= 0)
                    {
                        MenuManagement.RightError("Invalid year (cannot be in the future).");
                        MenuManagement.RightEnd(); continue;
                    }
                    step = 2;
                }
                else if (step == 2)
                {
                    var input = (MenuManagement.RightAsk("Duration in minutes (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 1; continue; }
                    if (!int.TryParse(input, out duration) || duration <= 0)
                    {
                        MenuManagement.RightError("Duration must be a positive number.");
                        MenuManagement.RightEnd(); continue;
                    }
                    step = 3;
                }
                else if (step == 3)
                {
                    var input = (MenuManagement.RightAsk("Budget (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 2; continue; }
                    if (!decimal.TryParse(input, out budget) || budget <= 0)
                    {
                        MenuManagement.RightError("Budget must be a positive number.");
                        MenuManagement.RightEnd(); continue;
                    }
                    step = 4;
                }
                else
                {
                    var genres = _genres.GetAll();
                    if (genres.Count == 0)
                    {
                        MenuManagement.RightWarn("No genres. Create a genre first.");
                        MenuManagement.RightEnd(); return;
                    }
                    MenuManagement.RightWriteLine("-- Genres --");
                    foreach (var g in genres.OrderBy(x => x.Id))
                        MenuManagement.RightWriteLine($"ID:[{g.Id}] {g.Name}");
                    MenuManagement.RightWriteLine("");

                    var input = (MenuManagement.RightAsk("Genre Id (menu/back): ") ?? "").Trim();
                    if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 3; continue; }

                    if (!int.TryParse(input, out genreId))
                    {
                        MenuManagement.RightError("Input a number.");
                        MenuManagement.RightEnd(); continue;
                    }

                    try
                    {
                        _movies.Create(new Movie
                        {
                            Title = title,
                            ReleaseYear = releaseYear,
                            Duration = duration,
                            Budget = budget,
                            GenreId = genreId
                        });
                        MenuManagement.RightSuccess("Movie created.");
                    }
                    catch (Exception ex)
                    {
                        MenuManagement.RightError($"Error: {ex.Message}");
                    }
                    MenuManagement.RightEnd(); return;
                }

                MenuManagement.RightEnd();
            }
        }

        // 4. Show All Genres
        public void ShowAllGenres()
        {
            MenuManagement.RightBegin("All Genres");
            var list = _genres.GetAll();
            if (list.Count == 0)
            {
                MenuManagement.RightWarn("No genres.");
                MenuManagement.RightEnd(); return;
            }
            foreach (var g in list.OrderBy(x => x.Id))
                MenuManagement.RightWriteLine($"ID:[{g.Id}] Name:{g.Name} | Movies:{g.Movies?.Count ?? 0}");
            MenuManagement.RightEnd();
        }

        // 5. Show All Actors
        public void ShowAllActors()
        {
            MenuManagement.RightBegin("All Actors");
            var list = _actors.GetAll();
            if (list.Count == 0)
            {
                MenuManagement.RightWarn("No actors.");
                MenuManagement.RightEnd(); return;
            }
            foreach (var a in list.OrderBy(x => x.Id))
                MenuManagement.RightWriteLine($"ID:[{a.Id}] {a.Name} {a.Surname} | Country:{a.Country} | Movies:{a.MovieActors?.Count ?? 0}");
            MenuManagement.RightEnd();
        }

        // 6. Show All Movies
        public void ShowAllMovies()
        {
            MenuManagement.RightBegin("All Movies");
            var list = _movies.GetAll();
            if (list.Count == 0)
            {
                MenuManagement.RightWarn("No movies.");
                MenuManagement.RightEnd(); return;
            }
            foreach (var m in list.OrderBy(x => x.Id))
            {
                var genreName = m.Genre?.Name ?? $"GenreId={m.GenreId}";
                MenuManagement.RightWriteLine(
                    $"ID:[{m.Id}] {m.Title} | Genre:{genreName} | Year:{m.ReleaseYear} | Actors:{m.MovieActors?.Count ?? 0} | Budget:{m.Budget}");
            }
            MenuManagement.RightEnd();
        }

        // 7. Show Movie Details
        public void ShowMovieDetails()
        {
            MenuManagement.RightBegin("Show Movie Details");

            var movies = _movies.GetAll().OrderBy(m => m.Id).ToList();
            if (movies.Count == 0)
            {
                MenuManagement.RightWarn("No movies.");
                MenuManagement.RightEnd(); return;
            }
            foreach (var m in movies)
                MenuManagement.RightWriteLine($"ID:[{m.Id}] {m.Title} ({m.ReleaseYear})");
            MenuManagement.RightWriteLine("");

            var input = (MenuManagement.RightAsk("Movie Id (menu): ") ?? "").Trim();
            if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }

            if (!int.TryParse(input, out var id))
            {
                MenuManagement.RightError("Input a number.");
                MenuManagement.RightEnd(); return;
            }

            var movie = _movies.GetById(id);
            if (movie is null)
            {
                MenuManagement.RightError("Movie not found.");
                MenuManagement.RightEnd(); return;
            }

            MenuManagement.RightWriteLine("");
            MenuManagement.RightWriteLine($"Title   : {movie.Title}");
            MenuManagement.RightWriteLine($"Genre   : {movie.Genre?.Name ?? "N/A"}");
            MenuManagement.RightWriteLine($"Year    : {movie.ReleaseYear}");
            MenuManagement.RightWriteLine($"Duration: {movie.Duration} min");
            MenuManagement.RightWriteLine($"Budget  : {movie.Budget}");
            MenuManagement.RightWriteLine("-- Actors --");

            if (movie.MovieActors == null || movie.MovieActors.Count == 0)
            {
                MenuManagement.RightWriteLine("(no actors assigned)");
            }
            else
            {
                foreach (var ma in movie.MovieActors)
                    MenuManagement.RightWriteLine($" - {ma.Actor?.Name} {ma.Actor?.Surname}");
            }

            MenuManagement.RightEnd();
        }

        // 8. Assign Actor To Movie
        public void AssignActorToMovie()
        {
            int step = 0;
            int movieId = 0, actorId = 0;

            while (true)
            {
                MenuManagement.RightBegin("Assign Actor To Movie");

                if (step == 0)
                {
                    var movies = _movies.GetAll().OrderBy(m => m.Id).ToList();
                    if (movies.Count == 0)
                    {
                        MenuManagement.RightWarn("No movies.");
                        MenuManagement.RightEnd(); return;
                    }
                    MenuManagement.RightWriteLine("-- Movies --");
                    foreach (var m in movies)
                        MenuManagement.RightWriteLine($"ID:[{m.Id}] {m.Title}");
                    MenuManagement.RightWriteLine("");

                    var s = (MenuManagement.RightAsk("Movie Id (menu): ") ?? "").Trim();
                    if (s.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (!int.TryParse(s, out movieId))
                    {
                        MenuManagement.RightError("Input a number.");
                        MenuManagement.RightEnd(); continue;
                    }
                    step = 1;
                    MenuManagement.RightEnd();
                    continue;
                }
                else
                {
                    var actors = _actors.GetAll().OrderBy(a => a.Id).ToList();
                    if (actors.Count == 0)
                    {
                        MenuManagement.RightWarn("No actors.");
                        MenuManagement.RightEnd(); return;
                    }
                    MenuManagement.RightWriteLine("-- Actors --");
                    foreach (var a in actors)
                        MenuManagement.RightWriteLine($"ID:[{a.Id}] {a.Name} {a.Surname}");
                    MenuManagement.RightWriteLine("");

                    var s = (MenuManagement.RightAsk("Actor Id (menu/back): ") ?? "").Trim();
                    if (s.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }
                    if (s.Equals("back", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); step = 0; continue; }
                    if (!int.TryParse(s, out actorId))
                    {
                        MenuManagement.RightError("Input a number.");
                        MenuManagement.RightEnd(); continue;
                    }

                    try
                    {
                        _movies.AssignActorToMovie(movieId, actorId);
                        MenuManagement.RightSuccess("Actor assigned to movie.");
                    }
                    catch (Exception ex)
                    {
                        MenuManagement.RightError($"Error: {ex.Message}");
                    }
                    MenuManagement.RightEnd(); return;
                }
            }
        }

        // 9. Show Movie Actors
        public void ShowMovieActors()
        {
            MenuManagement.RightBegin("Show Movie Actors");

            var movies = _movies.GetAll().OrderBy(m => m.Id).ToList();
            if (movies.Count == 0)
            {
                MenuManagement.RightWarn("No movies.");
                MenuManagement.RightEnd(); return;
            }
            foreach (var m in movies)
                MenuManagement.RightWriteLine($"ID:[{m.Id}] {m.Title}");
            MenuManagement.RightWriteLine("");

            var input = (MenuManagement.RightAsk("Movie Id (menu): ") ?? "").Trim();
            if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }

            if (!int.TryParse(input, out var movieId))
            {
                MenuManagement.RightError("Input a number.");
                MenuManagement.RightEnd(); return;
            }

            try
            {
                var actors = _movies.GetMovieActors(movieId);
                if (actors.Count == 0)
                {
                    MenuManagement.RightWarn("No actors assigned to this movie.");
                    MenuManagement.RightEnd(); return;
                }

                MenuManagement.RightWriteLine("");
                foreach (var a in actors)
                {
                    var age = DateTime.Now.Year - a.BirthDate.Year;
                    if (DateTime.Now.DayOfYear < a.BirthDate.DayOfYear) age--;
                    MenuManagement.RightWriteLine($"{a.Name} {a.Surname} | Country:{a.Country} | Age:{age}");
                }
            }
            catch (Exception ex)
            {
                MenuManagement.RightError($"Error: {ex.Message}");
            }

            MenuManagement.RightEnd();
        }

        // 10. Search Movie
        public void SearchMovie()
        {
            MenuManagement.RightBegin("Search Movie");

            var keyword = (MenuManagement.RightAsk("Keyword (menu): ") ?? "").Trim();
            if (keyword.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }

            var results = _movies.Search(keyword);
            if (results.Count == 0)
            {
                MenuManagement.RightWarn("No movies found.");
                MenuManagement.RightEnd(); return;
            }

            MenuManagement.RightWriteLine("");
            foreach (var m in results)
            {
                var genreName = m.Genre?.Name ?? $"GenreId={m.GenreId}";
                MenuManagement.RightWriteLine($"ID:[{m.Id}] {m.Title} ({m.ReleaseYear}) | Genre:{genreName}");
            }

            MenuManagement.RightEnd();
        }

        // 11. Movie Statistics
        public void MovieStatistics()
        {
            MenuManagement.RightBegin("Movie Statistics");

            MovieStatisticsDto stats = _movies.GetStatistics();

            MenuManagement.RightWriteLine($"Total Movies : {stats.TotalMovies}");
            MenuManagement.RightWriteLine($"Total Actors : {stats.TotalActors}");
            MenuManagement.RightWriteLine($"Total Genres : {stats.TotalGenres}");
            MenuManagement.RightWriteLine($"Avg Budget   : {stats.AverageBudget:0.##}");
            MenuManagement.RightWriteLine($"Longest Movie: {stats.LongestMovieTitle} ({stats.LongestMovieDuration} min)");
            MenuManagement.RightWriteLine("");
            MenuManagement.RightWriteLine("-- Movies by Genre --");
            foreach (var kv in stats.MovieCountByGenre)
                MenuManagement.RightWriteLine($" {kv.Key}: {kv.Value}");
            MenuManagement.RightWriteLine("");
            MenuManagement.RightWriteLine($"Most Actors Movie: {stats.MostActorsMovieTitle} ({stats.MostActorsCount} actors)");
            MenuManagement.RightWriteLine($"Most Active Actor: {stats.MostActiveActorFullName} ({stats.MostActiveActorMovieCount} movies)");

            MenuManagement.RightEnd();
        }

        // 12. Delete Movie (Soft Delete)
        public void DeleteMovie()
        {
            MenuManagement.RightBegin("Delete Movie (Soft Delete)");

            var movies = _movies.GetAll().OrderBy(m => m.Id).ToList();
            if (movies.Count == 0)
            {
                MenuManagement.RightWarn("No movies.");
                MenuManagement.RightEnd(); return;
            }
            foreach (var m in movies)
                MenuManagement.RightWriteLine($"ID:[{m.Id}] {m.Title}");
            MenuManagement.RightWriteLine("");

            var input = (MenuManagement.RightAsk("Movie Id (menu): ") ?? "").Trim();
            if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }

            if (!int.TryParse(input, out var id))
            {
                MenuManagement.RightError("Input a number.");
                MenuManagement.RightEnd(); return;
            }

            try
            {
                _movies.SoftDelete(id);
                MenuManagement.RightSuccess("Movie soft-deleted.");
            }
            catch (Exception ex)
            {
                MenuManagement.RightError($"Error: {ex.Message}");
            }

            MenuManagement.RightEnd();
        }

        // 13. Restore Movie
        public void RestoreMovie()
        {
            MenuManagement.RightBegin("Restore Movie");

            var input = (MenuManagement.RightAsk("Movie Id (menu): ") ?? "").Trim();
            if (input.Equals("menu", StringComparison.OrdinalIgnoreCase)) { MenuManagement.RightEnd(); return; }

            if (!int.TryParse(input, out var id))
            {
                MenuManagement.RightError("Input a number.");
                MenuManagement.RightEnd(); return;
            }

            try
            {
                _movies.Restore(id);
                MenuManagement.RightSuccess("Movie restored.");
            }
            catch (Exception ex)
            {
                MenuManagement.RightError($"Error: {ex.Message}");
            }

            MenuManagement.RightEnd();
        }
    }
}
