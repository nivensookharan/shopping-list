using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{

    public class UserViewModel
    {
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime LastUpdatedDateTimeStamp { get; set; }
    }
}
