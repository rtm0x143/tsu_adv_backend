using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Notifications.Domain;
using Notifications.Features.Notifications.Common;

namespace Notifications.Converters
{
    public static partial class NotificationMapper
    {
        public static ExtensibleNotification AdaptToExtensible(this Notification p1)
        {
            return p1 == null ? null : new ExtensibleNotification()
            {
                NotifyTime = p1.NotifyTime,
                Topic = p1.Topic.ToString(),
                Title = p1.Title,
                Extensions = funcMain1(p1.Payload)
            };
        }
        public static Expression<Func<Notification, ExtensibleNotification>> ProjectToExtensible => p3 => new ExtensibleNotification()
        {
            NotifyTime = p3.NotifyTime,
            Topic = p3.Topic.ToString(),
            Title = p3.Title,
            Extensions = p3.Payload
        };
        
        private static Dictionary<string, object> funcMain1(Dictionary<string, object> p2)
        {
            if (p2 == null)
            {
                return null;
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            
            if (object.ReferenceEquals(p2, result))
            {
                return result;
            }
            
            IEnumerator<KeyValuePair<string, object>> enumerator = p2.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, object> kvp = enumerator.Current;
                
                string key = kvp.Key;
                result[key] = kvp.Value;
            }
            
            return result;
            
        }
    }
}