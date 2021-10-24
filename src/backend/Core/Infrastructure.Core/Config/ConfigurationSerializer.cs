using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Core.Config
{
    public static class ConfigurationSerializer
    {
        public static JToken Serialize(IConfiguration config)
        {
            var obj = new JObject();
            foreach (var child in config.GetChildren())
            {
                obj.Add(child.Key, Serialize(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
                return new JValue(section.Value);

            return obj;
        }
    }
}