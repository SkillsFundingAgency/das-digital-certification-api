using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AdminActionsConfiguration : IEntityTypeConfiguration<AdminActions>
    {
        public void Configure(EntityTypeBuilder<AdminActions> builder)
        {
            builder
                .ToTable(nameof(AdminActions))
                .HasKey(x => x.Id);

            builder
                .Property(aa => aa.Action)
                .HasConversion<string>()
                .HasColumnName("Action");

            builder
                .HasOne(aa => aa.UserAction)
                .WithMany(ua => ua.AdminActions)
                .HasForeignKey(aa => aa.UserActionId);
        }
    }
}
