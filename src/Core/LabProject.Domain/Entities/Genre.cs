
using LabProject.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Domain.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; } = new();
    }

}
