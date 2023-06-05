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
    public class ExamEntityTypeConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            // set primary key
            builder.HasKey(o => o.Id);


            // set identity
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreateBy).HasMaxLength(150);
            builder.Property(x => x.Title).HasMaxLength(255);

            builder.Property(e => e.IsPublic)
                .HasDefaultValue(false);


            // set foreign key
            builder.HasMany<ExamCategory>().WithOne().HasForeignKey(x => x.ExamId);
            builder.HasMany<QuestionExam>().WithOne().HasForeignKey(x => x.ExamId);
            builder.HasMany<Test>().WithOne().HasForeignKey(x => x.ExamId);
        }
    }
}
