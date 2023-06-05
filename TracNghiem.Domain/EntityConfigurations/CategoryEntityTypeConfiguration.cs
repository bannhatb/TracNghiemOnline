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
    class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            // set primary key
            builder.HasKey(o => o.Id);


            // set identity
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.CategoryName)
                .HasMaxLength(255);

            

            // set foreign key
            builder.HasMany<QuestionCategory>().WithOne().HasForeignKey(x => x.CategoryId);
            builder.HasMany<ExamCategory>().WithOne().HasForeignKey(x => x.CategoryId);
        }
    }
}
