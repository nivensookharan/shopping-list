using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class ShoppingListViewModel
    {
        public Guid ShoppingListId { get; set; }
         public Guid? UserId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedDateTimeStamp { get; set; }
   
    }
}
