using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public static class MongoDBConfigurationExtensions
    {
        public static IConfigurationBuilder AddMongoDBConfigProvider<TSettingsObject>(this IConfigurationBuilder builder, SharpSettingsMongoDataStore<TSettingsObject> store, string settingsId, bool reloadOnChange = true) where TSettingsObject : WatchableSettings<string>
        {
            return builder.Add(new MongoDBConfigurationSource<TSettingsObject>(store, settingsId, reloadOnChange));
        }
    }
}