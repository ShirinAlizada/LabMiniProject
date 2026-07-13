using LabProject.Application.Interfaces.IServices;
using LabProject.Application.Services;
using LabProject.Persistence.Contexts;
using LabProject.Persistence.Repositories;
using LabProject.Presentation.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LabProject.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new AppDbContext();
            db.Database.Migrate();

            var genreRepo = new GenreRepository(db);
            var actorRepo = new ActorRepository(db);
            var movieRepo = new MovieRepository(db);
            var movieActorRepo = new MovieActorRepository(db);

            IGenreService genreService = new GenreService(genreRepo);
            IActorService actorService = new ActorService(actorRepo);
            IMovieService movieService = new MovieService(movieRepo, genreRepo, actorRepo, movieActorRepo);

            var manage = new ManageMetods(genreService, actorService, movieService);

            MenuManagement.Run(manage);
        }
    }
}
