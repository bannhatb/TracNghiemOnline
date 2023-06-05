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
    public class TestQuestionResultEntityTypeConfiguration : IEntityTypeConfiguration<TestQuestionResult>
    {
        public void Configure(EntityTypeBuilder<TestQuestionResult> builder)
        {
            builder.ToTable("TestQuestionResults");

            // set primary key
            builder.HasKey(o => o.Id);


            // set identity
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
        }
    }
}
