using Domain.DomainEvents;
using System.Collections.Generic;
using Tactical.DDD;

namespace PolymorphicWebAPI.Domain.Entities
{
    public class ItemCategory : Tactical.DDD.EventSourcing.AggregateRoot<ItemCategoryId>
    {
        public override ItemCategoryId Id { get; protected set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public string Type { get; set; }

        public ItemCategory(IEnumerable<IDomainEvent> events) : base(events)
        {
        }

      

        private ItemCategory()
        {

        }

        public static ItemCategory CreateNewItemCategory(string categoryName, string description, int quantity)
        {

            var itemCategory = new ItemCategory();
            itemCategory.Apply(new ItemCategoryCreated(new ItemCategoryId().ToString(),
                categoryName, description, quantity));
           

            return itemCategory;
        }

       


        public  void UpdateItemCategory(string id, string categoryName, string description, int quantity)
        {
            
            Apply(new ItemCategoryUpdated(id, categoryName, description, quantity));

        }

        public void RemoveItemCategory(string id, string categoryName, string description, int quantity)
        {

           
            Apply(new ItemCategoryRemoved(id, categoryName, description, quantity));

        }

        
        public void On(DomainEvent @event)
        {
            
            Id = new ItemCategoryId(@event.ItemCategoryId);
            CategoryName = @event.CategoryName;
            Description = @event.Description;
            Quantity = @event.Quantity;
            
            
        }

      



    }
}
