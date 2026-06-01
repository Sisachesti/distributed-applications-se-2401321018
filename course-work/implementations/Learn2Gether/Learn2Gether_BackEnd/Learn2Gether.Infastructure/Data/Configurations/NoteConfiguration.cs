using Learn2Gether.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.CreatedAt)
                .IsRequired();

            builder.Property(n => n.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(n => n.Lecture)
                .WithMany(l => l.LectureNotes)
                .HasForeignKey(n => n.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Student)
                .WithMany(u => u.StudentNotes)
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
