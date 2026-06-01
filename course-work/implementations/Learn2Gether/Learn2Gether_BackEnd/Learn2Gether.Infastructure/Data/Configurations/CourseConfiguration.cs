using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Learn2Gether.Shared.Common.EntityValidationConstants.CourseConstraints;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLength);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(MaxDescription);

            builder.Property(c => c.Rating)
                .HasDefaultValue(0)
                .HasMaxLength(MaxRating);

            builder.Property(c => c.Duration)
                .IsRequired()
                .HasMaxLength(MaxDurationInMinutes);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(ImageUrlMaxChars);

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(c => c.IsCompleted)
                .HasDefaultValue(false);

            builder.HasOne(c => c.Instructor)
                .WithMany(u => u.InstructorCourses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.CourseModules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.CourseStudents)
                .WithOne(sc => sc.Course)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(CourseSeeder.Seed());
        }
    }
}
