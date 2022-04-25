using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SharpSettings.AspNet
{
    public static class SharpSettingsConfigurationExtensions
    {
        public static IConfigurationBuilder AddSharpSettingsConfigProvider<TId, TSettingsObject>(this IConfigurationBuilder builder, ILogger logger, ISettingsWatcher<TId, TSettingsObject> settingsWatcher) where TSettingsObject : WatchableSettings<TId>
        {
            return builder.Add(new SharpSettingsConfigurationSource<TId, TSettingsObject>(logger, settingsWatcher));
        }

        public static IConfigurationBuilder AddMongoDBConfigProvider<TId, TSettingsObject>(this IConfigurationBuilder builder, ILoggerFactory loggerFactory, ISettingsWatcher<TId, TSettingsObject> settingsWatcher) where TSettingsObject : WatchableSettings<TId>
        {
            return builder.Add(new SharpSettingsConfigurationSource<TId, TSettingsObject>(loggerFactory.CreateLogger<SharpSettingsConfigurationSource<TId, TSettingsObject>>(), settingsWatcher));
        }
    }
}