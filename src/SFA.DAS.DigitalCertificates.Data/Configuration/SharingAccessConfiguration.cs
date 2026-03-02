using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class SharingAccessConfiguration : IEntityTypeConfiguration<SharingAccess>
    {
        public void Configure(EntityTypeBuilder<SharingAccess> builder)
        {
            builder
                .ToTable(nameof(SharingAccess))
                .HasKey(x => x.Id);

            builder
                .HasOne(sa => sa.Sharing)
                .WithMany(s => s.SharingAccesses)
                .HasForeignKey(sa => sa.SharingId);
        }
    }
}
