using Learn2Gether.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Learn2Gether.Shared.Common.EntityValidationConstants.AnswerConstraints;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .HasMaxLength(TitleMaxLength);

            builder.Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(ContentMaxLength);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(a => a.LikesCount)
                .HasDefaultValue(0);

            builder.Property(a => a.DislikesCount)
                .HasDefaultValue(0);

            builder.Property(a => a.IsAccepted)
                .HasDefaultValue(false);

            builder.HasOne(a => a.Lecture)
                .WithMany(l => l.LectureAnswers)
                .HasForeignKey(a => a.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Question)
                .WithMany(q => q.QuestionAnswers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.User)
                .WithMany(u => u.UserAnswers)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
