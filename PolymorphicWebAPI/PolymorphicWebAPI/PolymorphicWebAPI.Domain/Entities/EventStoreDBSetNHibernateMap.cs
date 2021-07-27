
using FluentNHibernate.Mapping;
using PolymorphicWebAPI.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Domain.Entities
{
    public class EventStoreDBSetNHibernateMap: ClassMap<EventStoreDBSet>
    {
        public EventStoreDBSetNHibernateMap()
        {
            Table(TablesDocuments.EventStoreTableName);
            Id(x => x.Id);
            Map(x => x.Data).Length(10000);
            Map(x => x.Version);
            Map(x => x.CreatedAt);
            Map(x => x.Sequence).ReadOnly();
            Map(x => x.Name);
            Map(x => x.AggregateId);

        }
    }
}
