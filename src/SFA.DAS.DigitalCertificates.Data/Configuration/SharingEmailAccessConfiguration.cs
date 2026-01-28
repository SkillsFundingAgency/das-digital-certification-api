using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class SharingEmailAccessConfiguration : IEntityTypeConfiguration<SharingEmailAccess>
    {
        public void Configure(EntityTypeBuilder<SharingEmailAccess> builder)
        {
            builder
                .ToTable(nameof(SharingEmailAccess))
                .HasKey(x => x.Id);

            builder
                .HasOne(sea => sea.SharingEmail)
                .WithMany(se => se.SharingEmailAccesses)
                .HasForeignKey(sea => sea.SharingEmailId);
        }
    }
}