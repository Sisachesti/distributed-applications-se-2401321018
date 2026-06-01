using Learn2Gether.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(q => q.Content)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(q => q.AskedOn)
                .IsRequired();

            builder.Property(q => q.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(q => q.Lecture)
                .WithMany(l => l.LectureQuestions)
                .HasForeignKey(q => q.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.User)
                .WithMany(u => u.UserQuestions)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.QuestionAnswers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
