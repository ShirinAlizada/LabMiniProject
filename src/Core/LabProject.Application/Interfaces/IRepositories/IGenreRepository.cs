using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Interfaces.IRepositories
{
    public interface IGenreRepository
    {
        void Create(Genre genre);
        List<Genre> GetAll();
        Genre? GetById(int id);
        Genre? GetByName(string name);
    }
}
