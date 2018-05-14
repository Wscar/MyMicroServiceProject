using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    class ProjectViewEntityConfiguration : IEntityTypeConfiguration<Project.Domain.AggregatesModel.ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<ProjectViewer> builder)
        {
            builder.ToTable("ProjectViewers")
                .HasKey(x => new { x.Id,x.ProjectId });
           
            builder.Property(x => x.UserName).HasColumnType("varchar(50)");
        }
    }
}
