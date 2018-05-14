using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project.Domain.AggregatesModel.Project>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.Project> builder)
        {
            builder.ToTable("Projects")
                .HasKey(x=>x.Id);
            builder.Property(x => x.Area).HasColumnType("varchar(50)");

            builder.Property(x => x.City).HasColumnType("varchar(50)");
            builder.Property(x => x.Avatar).HasColumnType("varchar(500)");
            builder.Property(x => x.Company).HasColumnType("varchar(50)");
            builder.Property(x => x.FinPercentage).HasColumnType("varchar(50)");
            builder.Property(x => x.FinStage).HasColumnType("varchar(50)");
            builder.Property(x => x.FormatBPFile).HasColumnType("varchar(500)");
            builder.Property(x => x.Introduction).HasColumnType("varchar(500)");
            builder.Property(x => x.OriginBPFile).HasColumnType("varchar(500)");
            builder.Property(x => x.Province).HasColumnType("varchar(50)");
            builder.Property(x => x.Tags).HasColumnType("varchar(100)");
        }
    }
}
