using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class SharingConfiguration : IEntityTypeConfiguration<Sharing>
    {
        public void Configure(EntityTypeBuilder<Sharing> builder)
        {
            builder
                .ToTable(nameof(Sharing))
                .HasKey(x => x.Id);

            builder
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            builder
                .HasMany(s => s.SharingAccesses)
                .WithOne(sa => sa.Sharing)
                .HasForeignKey(sa => sa.SharingId);

            builder
                .HasMany(s => s.SharingEmails)
                .WithOne(se => se.Sharing)
                .HasForeignKey(se => se.SharingId);
        }
    }
}