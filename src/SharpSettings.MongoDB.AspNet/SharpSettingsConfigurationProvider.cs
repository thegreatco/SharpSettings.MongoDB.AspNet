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

        public override void Load()
        {
            var settings = _settingsWatcher.GetSettings();
            if (settings == null)
            {
                _logger.LogDebug("Retrieved settings were null. This can happen with async data stores.");
            }

		    var dbSettings = GetProperties(settings);
            if (dbSettings == null)
                return;
                
		    Data = dbSettings.ToDictionary(x => x.Item1, x => x.Item2.ToString());
        }

        public override void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        private static IDictionary<string, string> GetProperties(object obj, string currentFieldName = null)
        {
            if (obj == null) return null;
            var stuffToReturn = new List<Tuple<string, object>>();
            var type = obj.GetType();
            if (type.IsArray)
            {
                var objArray = (IEnumerable)obj;
                var i = 0;
                foreach(var it in objArray)
                {
                    var props = GetProperties(it).ToArray();
                    if (props.Any())
                        stuffToReturn.AddRange(props);
                    else
                        stuffToReturn.Add(new Tuple<string, object>(string.Join(":", currentFieldName, i), it));
                    i++;
                }
            }
            else
            {
                foreach (var property in type.GetTypeInfo().DeclaredProperties.Where(x => x.PropertyType.GetTypeInfo().IsClass))
                {
                    var val = property.GetValue(obj);
                    if(val == null) continue;
                    var props = GetProperties(val, currentFieldName == null ? property.Name : string.Join(":", currentFieldName, property.Name)).ToArray();
                    if (props.Any())
                        stuffToReturn.AddRange(props);
                    else
                        stuffToReturn.Add(new Tuple<string, object>(currentFieldName == null ? property.Name : string.Join(":", currentFieldName, property.Name), val));
                }
            }

            return stuffToReturn.Where(x => x != null);
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
                return dbSettings.Where(x => earlierKeys.Any(y => x.Key.StartsWith(y))).SelectMany(x => x);
            }
            return dbSettings
        }
    }
}