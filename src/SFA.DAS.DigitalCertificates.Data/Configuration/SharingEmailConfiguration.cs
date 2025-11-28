using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class SharingEmailConfiguration : IEntityTypeConfiguration<SharingEmail>
    {
        public void Configure(EntityTypeBuilder<SharingEmail> builder)
        {
            builder
                .ToTable(nameof(SharingEmail))
                .HasKey(x => x.Id);

            builder
                .HasOne(se => se.Sharing)
                .WithMany(s => s.SharingEmails)
                .HasForeignKey(se => se.SharingId);

            builder
                .HasMany(se => se.SharingEmailAccesses)
                .WithOne(sea => sea.SharingEmail)
                .HasForeignKey(sea => sea.SharingEmailId);
        }
    }
}