using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectVisibleEntityConfiguration : IEntityTypeConfiguration<Project.Domain.AggregatesModel.ProjectVisibleRule>
    {
        void IEntityTypeConfiguration<ProjectVisibleRule>.Configure(EntityTypeBuilder<ProjectVisibleRule> builder)
        {
            builder.ToTable("ProjectVisibleRules")
                .HasKey(x => new {x.Id,x.ProjectId });
          
            builder.Property(x => x.Tags).HasColumnType("varchar(100)");
           
        }
    }
}
