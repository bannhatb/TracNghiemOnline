using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.EntityConfigurations
{
    public class TestEntityTypeConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Tests");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreateBy).HasMaxLength(150);
            builder.Property(x => x.Password).HasMaxLength(20);

            builder.Property(x => x.StartAt).HasColumnType("datetime");
            builder.Property(x => x.EndAt).HasColumnType("datetime");

            builder.Property(x => x.HideAnswer).HasDefaultValue(true);
            builder.Property(x => x.SuffleQuestion).HasDefaultValue(false);

            //relation
            builder.HasMany<TestUser>().WithOne().HasForeignKey(x => x.TestId);
            builder.HasMany<TestQuestion>().WithOne().HasForeignKey(x => x.TestId);
        }
    }
}
