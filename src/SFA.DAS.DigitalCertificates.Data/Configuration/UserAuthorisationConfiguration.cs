using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class UserAuthorisationConfiguration : IEntityTypeConfiguration<UserAuthorisation>
    {
        public void Configure(EntityTypeBuilder<UserAuthorisation> builder)
        {
            builder.ToTable(nameof(UserAuthorisation))
                .HasKey(x => x.Id);

            builder.HasOne(e => e.User)
                .WithOne(u => u.UserAuthorisation)
                .HasForeignKey<UserAuthorisation>(a => a.UserId);
        }
    }
}