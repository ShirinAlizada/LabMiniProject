using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Interfaces.IRepositories
{
    public interface IMovieRepository
    {
        void Create(Movie movie);
        List<Movie> GetAll(bool includeDeleted = false);
        Movie? GetById(int id, bool includeDeleted = false);
        List<Movie> Search(string keyword);
        void Update(Movie movie);
    }
}
