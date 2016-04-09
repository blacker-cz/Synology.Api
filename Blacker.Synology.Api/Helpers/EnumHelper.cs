using System;
using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Helpers
{
    static class EnumHelper
    {
        public static T GetValueFromEnumMember<T>(string value)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                if (attribute != null && attribute.Value == value)
                {
                    return (T)field.GetValue(null);
                }
            }

            return default(T);
        }
    }
}
