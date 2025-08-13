using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.DigitalCertificates.Data.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Entities;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Data
{
    [ExcludeFromCodeCoverage]
    public class DigitalCertificatesDataContext : DbContext,
        IUserEntityContext
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly ApplicationSettings? _configuration;
        private readonly ChainedTokenCredential? _chainedTokenCredentialProvider;

        public virtual DbSet<User> Users { get; set; }
        
        DbSet<User> IEntityContext<User>.Entities => Users;
        
        public DigitalCertificatesDataContext(IOptions<ApplicationSettings>? config,
            DbContextOptions<DigitalCertificatesDataContext> options)
            : base(options)
        {
            _configuration = config?.Value;
        }

        public DigitalCertificatesDataContext(IOptions<ApplicationSettings>? config,
            DbContextOptions<DigitalCertificatesDataContext> options,
            ChainedTokenCredential chainedTokenCredentialProvider) 
            : base(options)
        {
            _configuration = config?.Value;
            _chainedTokenCredentialProvider = chainedTokenCredentialProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_chainedTokenCredentialProvider != null)
            {
                var connection = new SqlConnection
                {
                    ConnectionString = _configuration?.DbConnectionString,
                    AccessToken = _chainedTokenCredentialProvider
                        .GetTokenAsync(new TokenRequestContext(scopes: [AzureResource]))
                        .GetAwaiter().GetResult().Token
                };

                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
