using Mapster;
using Notifications.Domain;

namespace Notifications.Features.Notifications.Common;

public class ExtensibleNotificationMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Notification, ExtensibleNotification>()
            .Map(dest => dest.Extensions, src => src.Payload)
            .GenerateMapper(MapType.Projection | MapType.Map);
    }
}