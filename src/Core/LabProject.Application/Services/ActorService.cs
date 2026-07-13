using LabProject.Application.Interfaces.IRepositories;
using LabProject.Application.Interfaces.IServices;
using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actors;

        public ActorService(IActorRepository actors)
        {
            _actors = actors;
        }

        // 2. Create Actor
        public void Create(Actor actor)
        {
            if (actor is null)
                throw new ArgumentNullException(nameof(actor), "Actor məlumatları boş ola bilməz.");

            if (string.IsNullOrWhiteSpace(actor.Name))
                throw new ArgumentException("Actor adı boş ola bilməz!", nameof(actor.Name));

            if (string.IsNullOrWhiteSpace(actor.Surname))
                throw new ArgumentException("Actor soyadı boş ola bilməz!", nameof(actor.Surname));

            if (string.IsNullOrWhiteSpace(actor.Country))
                throw new ArgumentException("Ölkə boş ola bilməz!", nameof(actor.Country));

            // Biznes Qaydası: Doğum tarixi gələcəkdə ola bilməz
            if (actor.BirthDate >= DateTime.Now)
                throw new ArgumentException("Doğum tarixi bugünkü tarixdən böyük ola bilməz!", nameof(actor.BirthDate));

            actor.Name = actor.Name.Trim();
            actor.Surname = actor.Surname.Trim();
            actor.Country = actor.Country.Trim();

            _actors.Create(actor);
        }

        // 5. Show All Actors
        public List<Actor> GetAll()
        {
            return _actors.GetAll();
        }

        public Actor? GetById(int id)
        {
            return _actors.GetById(id);
        }
    }
}
