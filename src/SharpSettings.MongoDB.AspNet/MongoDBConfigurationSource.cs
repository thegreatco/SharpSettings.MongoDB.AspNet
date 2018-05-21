using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public class MongoDBConfigurationSource<TSettingsObject> : IConfigurationSource where TSettingsObject : WatchableSettings<string>
    {
        private readonly SharpSettingsMongoDataStore<TSettingsObject> Store;
        private readonly bool ReloadOnChange;
        private readonly string SettingsId;

        public MongoDBConfigurationSource(SharpSettingsMongoDataStore<TSettingsObject> store, string settingsId, bool reloadOnChange)
        {
            Store = store;
            SettingsId = settingsId;
            ReloadOnChange = reloadOnChange;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoDBConfigurationProvider<TSettingsObject>(Store, SettingsId, ReloadOnChange);
        }
    }
}