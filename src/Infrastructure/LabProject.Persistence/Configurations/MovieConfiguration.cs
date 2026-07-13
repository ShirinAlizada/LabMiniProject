using LabProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
            builder.Property(m => m.ReleaseYear).IsRequired();
            builder.Property(m => m.Duration).IsRequired();
            builder.Property(m => m.Budget).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(m => m.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(m => m.Genre)
                   .WithMany(g => g.Movies)
                   .HasForeignKey(m => m.GenreId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
