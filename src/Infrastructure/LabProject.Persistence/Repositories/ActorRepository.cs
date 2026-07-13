using LabProject.Application.Interfaces.IRepositories;
using LabProject.Domain.Entities;
using LabProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Persistence.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly AppDbContext _context;

        public ActorRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Actor actor)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
        }

        public List<Actor> GetAll()
        {
            // Movie Count / Statistics üçün MovieActors daxil edilir
            return _context.Actors
                           .Include(a => a.MovieActors)
                           .ToList();
        }

        public Actor? GetById(int id)
        {
            return _context.Actors
                           .Include(a => a.MovieActors)
                           .FirstOrDefault(a => a.Id == id);
        }
    }
}
