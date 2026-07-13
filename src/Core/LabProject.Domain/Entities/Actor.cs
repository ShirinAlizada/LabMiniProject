using LabProject.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Domain.Entities
{
    public class Actor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public List<MovieActor> MovieActors { get; set; } = new();
    }
}
