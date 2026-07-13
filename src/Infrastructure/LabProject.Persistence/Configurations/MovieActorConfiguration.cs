using LabProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Persistence.Configurations
{
    public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            builder.ToTable("MovieActors");

            // Composite Primary Key: MovieId + ActorId
            builder.HasKey(ma => new { ma.MovieId, ma.ActorId });

            builder.HasOne(ma => ma.Movie)
                   .WithMany(m => m.MovieActors)
                   .HasForeignKey(ma => ma.MovieId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ma => ma.Actor)
                   .WithMany(a => a.MovieActors)
                   .HasForeignKey(ma => ma.ActorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
