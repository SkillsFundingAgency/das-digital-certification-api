using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User))
                .HasKey(x => x.Id);

            builder.HasOne(e => e.UserAuthorisation)
                .WithOne(u => u.User)
                .HasForeignKey<UserAuthorisation>(a => a.UserId);
        }
    }
}