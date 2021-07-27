using System;
using Tactical.DDD;
namespace Domain.DomainEvents
{
   public class DomainEvent : IDomainEvent
    {
        public DateTime CreatedAt { get; set; }
        public virtual string ItemCategoryId { get; }
        public virtual string CategoryName { get; }
        public virtual string Description { get; }
        public virtual int Quantity { get; }
        public virtual string Type { get; }

        public DomainEvent(string itemCategoryId, string categoryName, string description, int quantity, string type)
        {
            CreatedAt = DateTime.UtcNow;
            ItemCategoryId = itemCategoryId;
            CategoryName = categoryName;
            Description = description;
            Quantity = quantity;
            Type = type;
        }

       
    }
}
