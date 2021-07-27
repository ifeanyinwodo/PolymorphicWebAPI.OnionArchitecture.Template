
namespace Domain.DomainEvents
{
    public class ItemCategoryCreated : DomainEvent
    {
        public override string ItemCategoryId { get; }
        public override string CategoryName { get; }
        public override string Description { get; }
        public override int Quantity { get; }

        public ItemCategoryCreated(string itemCategoryId, string categoryName, string description, int quantity):base(itemCategoryId,categoryName,description,quantity,null)
        {
            ItemCategoryId = itemCategoryId;
            CategoryName = categoryName;
            Description = description;
            Quantity = quantity;
        }
    }
}
