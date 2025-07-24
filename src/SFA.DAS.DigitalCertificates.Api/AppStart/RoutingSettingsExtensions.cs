using NServiceBus;
using SFA.DAS.Notifications.Messages.Commands;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class RoutingSettingsExtensions
    {
        private const string NotificationsMessageHandler = "SFA.DAS.Notifications.MessageHandlers";

        public static void AddRouting(this RoutingSettings routingSettings)
        {
            routingSettings.RouteToEndpoint(typeof(SendEmailCommand), NotificationsMessageHandler);
        }
    }
}
