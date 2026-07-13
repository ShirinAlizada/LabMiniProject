using LabProject.Application.Interfaces.IRepositories;
using LabProject.Domain.Entities;
using LabProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Persistence.Repositories
{
    public class MovieActorRepository : IMovieActorRepository
    {
        private readonly AppDbContext _context;

        public MovieActorRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(MovieActor movieActor)
        {
            _context.MovieActors.Add(movieActor);
            _context.SaveChanges();
        }

        public bool Exists(int movieId, int actorId)
        {
            return _context.MovieActors
                           .Any(ma => ma.MovieId == movieId && ma.ActorId == actorId);
        }

        public List<MovieActor> GetByMovieId(int movieId)
        {
            return _context.MovieActors
                           .Include(ma => ma.Actor)
                           .Where(ma => ma.MovieId == movieId)
                           .ToList();
        }
    }
}
