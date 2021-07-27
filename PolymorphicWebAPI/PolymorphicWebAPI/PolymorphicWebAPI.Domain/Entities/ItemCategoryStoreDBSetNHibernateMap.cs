using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Domain.Entities
{
    public class ItemCategoryStoreDBSetNHibernateMap: ClassMapping<ItemCategoryStoreDBSet>
    {
        public ItemCategoryStoreDBSetNHibernateMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Guid);
                x.Column("Id");
                x.UnsavedValue(Guid.Empty);

            });
            Property(c => c.CategoryName, x =>
            {
                x.Column("CategoryName");
                x.Type(NHibernateUtil.String);

            });

            Property(d => d.Description, x =>
            {
                x.Column("Description");
                x.Type(NHibernateUtil.String);
            });
            Property(q => q.Quantity, x =>
            {
                x.Column("Quantity");
                x.Type(NHibernateUtil.Int64);
            });
           

        }
    }
}
