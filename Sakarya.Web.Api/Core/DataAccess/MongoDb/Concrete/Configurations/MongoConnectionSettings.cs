using MongoDB.Driver;

namespace Core.DataAccess.MongoDb.Concrete.Configurations
{
    public class MongoConnectionSettings
    {
        public MongoConnectionSettings(MongoClientSettings mongoClientSettings)
        {
            MongoClientSettings = mongoClientSettings;
        }

        public MongoConnectionSettings()
        {
        }

        /// <summary>
        ///     To be set if the MongoClientSetting class is to be used.
        /// </summary>
        private MongoClientSettings MongoClientSettings { get; }

        public string user { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public string DatabaseName { get; set; }

        public MongoClientSettings GetMongoClientSettings()
        {
            return MongoClientSettings;
        }
    }
}