using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class ItemCategoryRemoved : DomainEvent
    {
        public override string ItemCategoryId { get; }
        public override string CategoryName { get; }
        public override string Description { get; }
        public override int Quantity { get; }

        public ItemCategoryRemoved(string itemCategoryId, string categoryName, string description, int quantity) : base(itemCategoryId, categoryName, description, quantity, null)
        {
            ItemCategoryId = itemCategoryId;
            CategoryName = categoryName;
            Description = description;
            Quantity = quantity;
        }
    }
}
