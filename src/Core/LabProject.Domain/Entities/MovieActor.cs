using LabProject.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Domain.Entities
{
    public class MovieActor : BaseEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int ActorId { get; set; }
        public Actor Actor { get; set; } = null!;
    }
}
