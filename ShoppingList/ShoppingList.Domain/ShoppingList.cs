using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Domain
{
    public class ShoppingList
    {
        public ShoppingList()
        {
            this.ShoppingListItems = new List<ShoppingListItem>();
            this.User = new User();
        }
        public Guid ShoppingListId { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedDateTimeStamp { get; set; }
        public DateTime CreatedDateTimeStamp { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ShoppingListItem> ShoppingListItems { get; set; }
        public virtual User User { get; set; }
    }
}
