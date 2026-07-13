using LabProject.Application.Interfaces.IRepositories;
using LabProject.Application.Interfaces.IServices;
using LabProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genres;

        public GenreService(IGenreRepository genres)
        {
            _genres = genres;
        }

        // 1. Create Genre
        public void Create(Genre genre)
        {
            if (genre is null)
                throw new ArgumentNullException(nameof(genre), "Genre məlumatları boş ola bilməz.");

            if (string.IsNullOrWhiteSpace(genre.Name))
                throw new ArgumentException("Genre adı boş ola bilməz!", nameof(genre.Name));

            genre.Name = genre.Name.Trim();

            // Biznes Qaydası: Eyni adda genre yaradıla bilməz
            var existing = _genres.GetByName(genre.Name);
            if (existing != null)
                throw new InvalidOperationException($"'{genre.Name}' adlı genre artıq mövcuddur!");

            _genres.Create(genre);
        }

        // 4. Show All Genres
        public List<Genre> GetAll()
        {
            return _genres.GetAll();
        }

        public Genre? GetById(int id)
        {
            return _genres.GetById(id);
        }
    }
}
