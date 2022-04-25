using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace SharpSettings.AspNet
{
    public class SharpSettingsConfigurationProvider<TId, TSettingsObject> : IConfigurationProvider where TSettingsObject : WatchableSettings<TId>
    {
        private readonly ILogger _logger;
        private readonly ISettingsWatcher<TId, TSettingsObject> _settingsWatcher;

        public SharpSettingsConfigurationProvider(ILogger logger, ISettingsWatcher<TId, TSettingsObject> settingsWatcher)
        {
            _logger = logger;
            _settingsWatcher = settingsWatcher;
        }

        public void Load()
        {
            var settings = _settingsWatcher.GetSettings();
            if (settings == null)
            {
                _logger.LogDebug("Retrieved settings were null. This can happen with async data stores.");
            }
        }

        public void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        private static IDictionary<string, string> GetProperties(object obj, string currentFieldName = null)
        {
            if (obj == null) return null;
            var stuffToReturn = new Dictionary<string, string>();
            var type = obj.GetType();
            if (type.IsArray)
            {
                var objArray = (IEnumerable)obj;
                var i = 0;
                foreach (var it in objArray)
                {
                    var props = GetProperties(it).ToArray();
                    if (props.Any())
                    {
                        foreach (var (key, value) in props)
                        {
                            stuffToReturn.Add(key, value);
                        }
                    }
                    else
                    {
                        stuffToReturn.Add(string.Join(":", currentFieldName, i), it as string);
                    }

                    i++;
                }
            }
            else
            {
                foreach (var property in type.GetTypeInfo().DeclaredProperties.Where(x => x.PropertyType.GetTypeInfo().IsClass))
                {
                    var val = property.GetValue(obj);
                    if (val == null) continue;
                    var props = GetProperties(val, currentFieldName == null ? property.Name : string.Join(":", currentFieldName, property.Name)).ToArray();
                    if (props.Any())
                    {
                        foreach (var (key, value) in props)
                        {
                            stuffToReturn.Add(key, value);
                        }
                    }
                    else
                    {
                        stuffToReturn.Add(currentFieldName == null ? property.Name : string.Join(":", currentFieldName, property.Name), val as string);
                    }
                }
            }

            return stuffToReturn;
        }

        public bool TryGet(string key, out string value)
        {
            var settings = _settingsWatcher.GetSettings();
            if (settings == null)
            {
                _logger.LogDebug("Retrieved settings were null. This can happen with async data stores.");
            }
            var dbSettings = GetProperties(settings);
            if (dbSettings != null && dbSettings.ContainsKey(key))
            {
                value = dbSettings[key];
                return true;
            }

            value = default;
            return false;
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var settings = _settingsWatcher.GetSettings();
            if (settings == null)
            {
                _logger.LogDebug("Retrieved settings were null. This can happen with async data stores.");
            }
            var dbSettings = GetProperties(settings);
            if (dbSettings != null)
            {
                return dbSettings.Where(x => earlierKeys.Any(y => x.Key.StartsWith(y))).Select(x => x.Key);
            }
            return Array.Empty<string>();
        }
    }
}
