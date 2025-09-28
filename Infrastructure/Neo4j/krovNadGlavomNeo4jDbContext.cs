using Neo4j.Driver;

namespace krov_nad_glavom_api.Infrastructure.Neo4j
{
    public class krovNadGlavomNeo4jDbContext
    {
        public IDriver Driver;

        public krovNadGlavomNeo4jDbContext(IDriver driver)
        {
            Driver = driver;
        }
    }
}