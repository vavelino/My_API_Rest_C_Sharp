using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Mappings
{
    class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Description)
               .IsRequired()
               .HasColumnType("varchar(1000)");

            builder.Property(p => p.Image)
               .IsRequired()
               .HasColumnType("varchar(100)");
            builder.ToTable("Products");

            builder.Property(p => p.Value)
               .IsRequired()
               .HasColumnType("decimal(10,2)");
            builder.ToTable("Products");
        }
    }
}
