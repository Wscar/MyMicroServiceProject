using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    class ProjectPropertyEntityConfiguation : IEntityTypeConfiguration<Project.Domain.AggregatesModel.ProjectProperties>
    {
        public void Configure(EntityTypeBuilder<ProjectProperties> builder)
        {
            builder.ToTable("ProjectProperties")
                 .HasKey(x => x.Key);
            builder.Property(x => x.Key).HasColumnType("varchar(100)");
            builder.Property(x => x.Text).HasColumnType("varchar(500)");
            builder.Property(x => x.Value).HasColumnType("varchar(500)");
        }
    }
}
