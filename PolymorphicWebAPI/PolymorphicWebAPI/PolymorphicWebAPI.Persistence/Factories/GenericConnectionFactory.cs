using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Factories
{
    public abstract class GenericConnectionFactory : IGenericConnectionFactory<IDbConnection>
    {
        public abstract string ConnectionString { get; }

        public virtual IDbConnection DBConnection()
        {
            throw new NotImplementedException();
        }
    }
}
