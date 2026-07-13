using LabProject.Application.DTO;
using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Interfaces.IServices
{
    public interface IMovieService
    {
        void Create(Movie movie);
        List<Movie> GetAll();
        Movie? GetById(int id);
        List<Movie> Search(string keyword);

        void AssignActorToMovie(int movieId, int actorId);
        List<Actor> GetMovieActors(int movieId);

        void SoftDelete(int movieId);
        void Restore(int movieId);

        MovieStatisticsDto GetStatistics();
    }
}
