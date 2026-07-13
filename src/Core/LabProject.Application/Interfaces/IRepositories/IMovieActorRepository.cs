using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Interfaces.IRepositories
{
    public interface IMovieActorRepository
    {
        void Create(MovieActor movieActor);
        bool Exists(int movieId, int actorId);
        List<MovieActor> GetByMovieId(int movieId);
    }
}
