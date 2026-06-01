using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Learn2Gether.Domain.Entities;

using static Learn2Gether.Shared.Common.EntityValidationConstants.ModuleConstraints;

namespace Learn2Gether.Infastructure.Data.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLength);

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(m => m.IsCompleted)
                .HasDefaultValue(false);

            builder.HasOne(m => m.Course)
                .WithMany(c => c.CourseModules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.ModuleLectures)
                .WithOne(l => l.Module)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
