using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.DigitalCertificates.Api.TaskQueue;
using SFA.DAS.DigitalCertificates.Application.Behaviours;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Data;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));

            services.AddValidatorsFromAssemblyContaining<GetUserQuery>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddScoped<IUserEntityContext>(s => s.GetRequiredService<DigitalCertificatesDataContext>());
        }
    }
}