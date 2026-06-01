using Learn2Gether.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Learn2Gether.Shared.Common.EntityValidationConstants.LectureConstraints;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLength);

            builder.Property(l => l.VideoUrl)
                .IsRequired()
                .HasMaxLength(VideoUrlMaxLength);

            builder.Property(l => l.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(l => l.IsCompleted)
                .HasDefaultValue(false);

            builder.HasOne(l => l.Module)
                .WithMany(m => m.ModuleLectures)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.LectureQuestions)
                .WithOne(q => q.Lecture)
                .HasForeignKey(q => q.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.LectureAnswers)
                .WithOne(a => a.Lecture)
                .HasForeignKey(a => a.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.LectureNotes)
                .WithOne(n => n.Lecture)
                .HasForeignKey(n => n.LectureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
