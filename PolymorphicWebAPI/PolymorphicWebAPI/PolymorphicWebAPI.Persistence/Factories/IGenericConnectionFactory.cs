using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Factories
{
    public interface IGenericConnectionFactory<out TConnection>  {

        string ConnectionString { get; }
        TConnection DBConnection();
       
    }

}
