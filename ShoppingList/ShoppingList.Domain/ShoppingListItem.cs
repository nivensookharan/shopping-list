using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Domain
{
    public class ShoppingListItem
    {
        public ShoppingListItem()
        {
            this.ShoppingList = new ShoppingList();
        }
        public Guid ShoppingListItemId { get; set; }
        public Guid ShoppingListId { get; set; }
        public Guid UserId { get; set; }

        public string DisplayName { get; set; }
        public string Description { get; set; }

        public string? ImageUrl { get; set; }


        public DateTime LastUpdatedDateTimeStamp { get; set; }
        public DateTime CreatedDateTimeStamp { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ShoppingList ShoppingList { get; set; }
    }
}
