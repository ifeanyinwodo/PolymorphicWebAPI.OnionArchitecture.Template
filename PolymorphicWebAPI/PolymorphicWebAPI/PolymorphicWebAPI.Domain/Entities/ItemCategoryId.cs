using System;
using Tactical.DDD;


namespace PolymorphicWebAPI.Domain.Entities
{
    public class ItemCategoryId: EntityId
    {
        private Guid _guid;

        public ItemCategoryId()
        {
            _guid = Guid.NewGuid();
        }

        public ItemCategoryId(string id)
        {
            _guid = Guid.Parse(id);
        }

        public override string ToString() => _guid.ToString();
    }
}
