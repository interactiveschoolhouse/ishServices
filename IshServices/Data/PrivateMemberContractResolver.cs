using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IshServices.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="https://danielwertheim.se/json-net-private-setters/"/>
    public class PrivateMemberContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
       MemberInfo member,
       MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}