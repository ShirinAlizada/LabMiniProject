using LabProject.Application.Interfaces.IRepositories;
using LabProject.Domain.Entities;
using LabProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Persistence.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public List<Movie> GetAll(bool includeDeleted = false)
        {
            var query = _context.Movies
                                .Include(m => m.Genre)
                                .Include(m => m.MovieActors)
                                    .ThenInclude(ma => ma.Actor)
                                .AsQueryable();

            if (!includeDeleted)
                query = query.Where(m => !m.IsDeleted);

            return query.ToList();
        }

        public Movie? GetById(int id, bool includeDeleted = false)
        {
            var query = _context.Movies
                                .Include(m => m.Genre)
                                .Include(m => m.MovieActors)
                                    .ThenInclude(ma => ma.Actor)
                                .AsQueryable();

            if (!includeDeleted)
                query = query.Where(m => !m.IsDeleted);

            return query.FirstOrDefault(m => m.Id == id);
        }

        public List<Movie> Search(string keyword)
        {
            var kw = (keyword ?? string.Empty).Trim().ToLower();

            return _context.Movies
                           .Include(m => m.Genre)
                           .Where(m => !m.IsDeleted && m.Title.ToLower().Contains(kw))
                           .ToList();
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
        }
    }
}
