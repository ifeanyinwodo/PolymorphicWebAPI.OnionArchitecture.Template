using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public interface INHibernateMapperSession : ISession
    {
        void BeginTransaction();
        Task Commit();
        Task Rollback();
        void CloseTransaction();
    }
}
