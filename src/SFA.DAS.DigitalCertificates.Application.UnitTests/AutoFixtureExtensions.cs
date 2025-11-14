using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.DigitalCertificates.Data;
using SFA.DAS.DigitalCertificates.Domain.Configuration;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.Application.UnitTests
{
    public static class AutofixtureExtensions
    {
        public static IFixture DigitalCertificatesFixture()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize(new DigitalCertificatesCustomization());
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public static IDateTimeProvider? DateTimeProvider { get; set; }

        public AutoMoqDataAttribute()
            : base(AutofixtureExtensions.DigitalCertificatesFixture)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoMoqInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineAutoDataAttribute(params object[] arguments)
            : base(() => AutofixtureExtensions.DigitalCertificatesFixture(), arguments)
        {
        }
    }

    public class DigitalCertificatesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new ApplicationSettingsBuilder());
            fixture.Customizations.Add(new DigitalCertificatesDataContextBuilder());
            fixture.Customizations.Add(new DbContextOptionsBuilder());
        }
    }

    public class DigitalCertificatesDataContextBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DigitalCertificatesDataContext))
            {
                var applicationSettings = context.Resolve(typeof(IOptions<ApplicationSettings>)) as IOptions<ApplicationSettings>;
                var dbContextOptions = context.Resolve(typeof(DbContextOptions<DigitalCertificatesDataContext>)) as DbContextOptions<DigitalCertificatesDataContext>;

                if (dbContextOptions != null)
                {
                    return GetDigitalCertificatesDataContext(applicationSettings, dbContextOptions);
                }
            }

            return new NoSpecimen();
        }

        public static DigitalCertificatesDataContext GetDigitalCertificatesDataContext(
            IOptions<ApplicationSettings>? applicationSettings,
            DbContextOptions<DigitalCertificatesDataContext> dbContextOptions)
        {
            var dbContext = new DigitalCertificatesDataContext(
                applicationSettings,
                dbContextOptions);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }

    public class DbContextOptionsBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DbContextOptions<DigitalCertificatesDataContext>))
            {
                var connection = new SqliteConnection("Filename=:memory:");
                connection.Open();

                // These options will be used by the context instances in this test suite, including the connection opened above.
                var contextOptions = new DbContextOptionsBuilder<DigitalCertificatesDataContext>()
                    .UseSqlite(connection)
                    .Options;
                
                return contextOptions;
            }

            return new NoSpecimen();
        }
    }

    public class ApplicationSettingsBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(IOptions<ApplicationSettings>))
            {
                var applicationSettings = new ApplicationSettings
                {
                };

                return Options.Create(applicationSettings);
            }

            return new NoSpecimen();
        }
    }
}
