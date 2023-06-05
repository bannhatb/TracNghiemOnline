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
    public class AnswerEntityTypeConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.AnswerContent)
                .HasMaxLength(255);

            builder.Property(x => x.RightAnswer).HasDefaultValue(false);

            //relation
            builder.HasMany<TestQuestionResult>()
                .WithOne().HasForeignKey(x => x.Choose).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
