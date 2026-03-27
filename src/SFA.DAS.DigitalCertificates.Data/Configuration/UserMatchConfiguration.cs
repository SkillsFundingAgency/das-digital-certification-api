using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class UserMatchConfiguration : IEntityTypeConfiguration<UserMatch>
    {
        public void Configure(EntityTypeBuilder<UserMatch> builder)
        {
            builder.ToTable(nameof(UserMatch))
                .HasKey(x => x.Id);

            builder.HasOne(um => um.User)
                .WithMany(u => u.UserMatches)
                .HasForeignKey(um => um.UserId);
        }
    }
}
