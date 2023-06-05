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
    public class QuestionEntityTypeConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            // set primary key
            builder.HasKey(o => o.Id);


            // set identity
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.QuestionContent).IsRequired()
                .HasMaxLength(255);

            // base attributes
            builder
                .Property(e => e.Explaint)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.RightCount).HasDefaultValue(1);

            // set foreign key
            builder.HasMany<QuestionExam>().WithOne().HasForeignKey(x => x.QuestionId);
            builder.HasMany<QuestionCategory>().WithOne().HasForeignKey(x => x.QuestionId);
            builder.HasMany<Answer>().WithOne().HasForeignKey(x => x.QuestionId);
            builder.HasMany<TestQuestion>().WithOne().HasForeignKey(x => x.QuestionId);
        }
    }
}
