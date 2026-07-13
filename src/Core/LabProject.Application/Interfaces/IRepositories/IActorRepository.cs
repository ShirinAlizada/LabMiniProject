using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Interfaces.IRepositories
{
    public interface IActorRepository
    {
        void Create(Actor actor);
        List<Actor> GetAll();
        Actor? GetById(int id);
    }
}
