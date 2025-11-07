using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
    {
        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            builder.ToTable(nameof(UserIdentity))
                .HasKey(x => x.Id);

            builder.HasOne(e => e.User)
                .WithMany(u => u.UserIdentities)
                .HasForeignKey(a => a.UserId);
        }
    }
}