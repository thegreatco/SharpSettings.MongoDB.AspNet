using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SharpSettings.AspNet
{
    public class SharpSettingsConfigurationSource<TId, TSettingsObject> : IConfigurationSource where TSettingsObject : WatchableSettings<TId>
    {
        private readonly ILogger Logger;
        private readonly ISettingsWatcher<TId, TSettingsObject> Store;

        /// <summary>
        /// Create a new <see cref="MongoDBConfigurationSource{TSettingsObject}"/>
        /// </summary>
        /// <param name="logger">A <see cref="ILogger"/> to log internal operations.</param>
        /// <param name="store">The <see cref=""/></param>
        public SharpSettingsConfigurationSource(ILogger logger, ISettingsWatcher<TId, TSettingsObject> store)
        {
            Logger = logger;
            Store = store;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SharpSettingsConfigurationProvider<TId, TSettingsObject>(Logger, Store);
        }
    }
}