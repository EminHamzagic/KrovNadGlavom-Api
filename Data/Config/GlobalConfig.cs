namespace krov_nad_glavom_api.Data.Config
{
    public class GlobalConfig
    {
        public GlobalConfig(ConfigurationManager configuration)
        {
            ConnectionString = configuration.GetConnectionString("DbConnectionString") ?? throw new InvalidOperationException("Missing DbConnectionString in configuration.");
            MongoDb = configuration.GetConnectionString("MongoDb") ?? throw new InvalidOperationException("Missing MongoDb in configuration.");
            AllowedOrigins = configuration.GetSection("AllowedOrigins").Get<List<string>>() ?? throw new InvalidOperationException("Missing AllowedOrigins in configuration.");
            JWTSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();
            ApplyEnvOverrides();
        }

        public string ConnectionString { get; set; }
        public string MongoDb { get; set; }
        public List<string> AllowedOrigins { get; set; }
        public JWTSettings JWTSettings { get; set; }
        private void ApplyEnvOverrides()
        {
            ApplyIfExists("DB_CONNECTION_STRING", value => ConnectionString = value);
            ApplyIfExists("ALLOWED_ORIGINS", value => AllowedOrigins = value.Split(",").ToList());
        }

        private void ApplyIfExists(string envVarName, Action<string> applyAction)
        {
            var value = Environment.GetEnvironmentVariable(envVarName);
            if (!string.IsNullOrEmpty(value))
            {
                applyAction(value);
            }
        }
    }
}