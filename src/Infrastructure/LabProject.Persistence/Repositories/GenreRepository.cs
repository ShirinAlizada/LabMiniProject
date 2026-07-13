using LabProject.Application.Interfaces.IRepositories;
using LabProject.Domain.Entities;
using LabProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Persistence.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }

        public List<Genre> GetAll()
        {
            // Movie Count üçün Movies daxil edilir
            return _context.Genres
                           .Include(g => g.Movies)
                           .ToList();
        }

        public Genre? GetById(int id)
        {
            return _context.Genres
                           .Include(g => g.Movies)
                           .FirstOrDefault(g => g.Id == id);
        }

        public Genre? GetByName(string name)
        {
            return _context.Genres
                           .FirstOrDefault(g => g.Name.ToLower() == name.ToLower());
        }
    }
}
