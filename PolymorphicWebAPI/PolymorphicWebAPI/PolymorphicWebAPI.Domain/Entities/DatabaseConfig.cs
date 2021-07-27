

namespace PolymorphicWebAPI.Domain.Entities
{
    public class DatabaseConfig
    {
        
        public string ConnectionString { get; set; }
       

        public string DataBaseType { get; set; }

        public string ORMType { get; set; }

        public string StoreType { get; set; }
    }
}
