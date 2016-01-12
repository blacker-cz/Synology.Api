using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ResponseConfigBase
    {
        protected internal ResponseConfigBase()
        {
        }

        internal IDictionary<string, object> GetChangedProperties()
        {
            return
                GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(p => new {p, attr = p.GetCustomAttributes(typeof (DataMemberAttribute), true)})
                    .Where(@t => @t.attr.Length == 1)
                    .Select(@t => new {Value = @t.p.GetValue(this), Attribute = @t.attr.First() as DataMemberAttribute})
                    .Concat(
                        GetType()
                            .GetProperties()
                            .Select(p => new {p, attr = p.GetCustomAttributes(typeof (DataMemberAttribute), true)})
                            .Where(@t => @t.attr.Length == 1)
                            .Select(@t => new {Value = @t.p.GetValue(this), Attribute = @t.attr.First() as DataMemberAttribute}))
                    .Where(p => p.Value != null)
                    .ToDictionary(p => p.Attribute.Name, p => p.Value);
        }
    }
}
