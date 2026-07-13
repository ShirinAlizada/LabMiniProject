using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Interfaces.IServices
{
    public interface IGenreService
    {
        void Create(Genre genre);
        List<Genre> GetAll();
        Genre? GetById(int id);
    }
}
