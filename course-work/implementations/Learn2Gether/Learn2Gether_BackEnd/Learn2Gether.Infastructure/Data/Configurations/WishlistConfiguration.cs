using Learn2Gether.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => new { w.StudentId, w.CourseId });

            builder.HasOne(w => w.Student)
                .WithMany(s => s.Wishlist)
                .HasForeignKey(w => w.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Course)
                .WithMany(c => c.Wishlist)
                .HasForeignKey(w => w.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
