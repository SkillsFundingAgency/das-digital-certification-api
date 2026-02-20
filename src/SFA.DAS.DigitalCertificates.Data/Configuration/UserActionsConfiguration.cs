using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class UserActionsConfiguration : IEntityTypeConfiguration<UserActions>
    {
        public void Configure(EntityTypeBuilder<UserActions> builder)
        {
            builder
                .ToTable(nameof(UserActions))
                .HasKey(x => x.Id);

            builder
                .Property(ua => ua.CertificateType)
                .HasConversion<string>()
                .HasColumnName("CertificateType");

            builder
                .Property(ua => ua.ActionType)
                .HasConversion<int>()
                .HasColumnName("ActionType");

            builder
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId);
        }
    }
}
