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
    public class TestUserEntityTypeConfiguration : IEntityTypeConfiguration<TestUser>
    {
        public void Configure(EntityTypeBuilder<TestUser> builder)
        {
            builder.ToTable("TestUsers");

            // set primary key
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Status).HasDefaultValue(0);
            builder.Property(x => x.TimeRemain).HasDefaultValue(0);
            // set identity
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
        }
    }
}
