using LabProject.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Domain.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public decimal Duration { get; set; }      // dəqiqə ilə
        public decimal Budget { get; set; }

        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public List<MovieActor> MovieActors { get; set; } = new();

        // Soft Delete üçün (12/13-cü menyu bəndləri buna görə lazımdır)
        public bool IsDeleted { get; set; } = false;
    }
}
