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
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // set primary key
            builder.HasKey(o => o.Id);


            // set identity
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.UserName)
                .HasMaxLength(255);

            // base attributes
            builder
                .Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.DateOfBirth).HasColumnType("datetime");

            builder.Property(x => x.IsBlock).HasDefaultValue(false);
            builder
                .Property(e => e.Email)
                .HasMaxLength(150);

            builder
                .Property(e => e.Avatar).IsRequired(false)
                .HasMaxLength(255);
            builder
                .Property(e => e.FirstName).IsRequired(false)
                .HasMaxLength(50);
            builder
                .Property(e => e.LastName).IsRequired(false)
                .HasMaxLength(50);


            // set foreign key
            builder.HasMany<UserRole>().WithOne().HasForeignKey(x => x.UserId);
            builder.HasMany<TestQuestion>().WithOne().HasForeignKey(x => x.UserId);
            builder.HasMany<TestUser>().WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
