using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    class ProjectContributorEntityConfiguration : IEntityTypeConfiguration<Project.Domain.AggregatesModel.ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<ProjectContributor> builder)
        {
            builder.ToTable("ProjectContributor")
                 .HasKey(x => new { x.Id,x.ProjectId });
            
            builder.Property(x => x.Avatar).HasColumnType("varchar(500)");
            builder.Property(x => x.UserName).HasColumnType("varchar(50)");
          
        }
    }
}
