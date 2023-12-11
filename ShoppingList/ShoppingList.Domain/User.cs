namespace ShoppingList.Domain
{
    public class User
    {
        public User()
        {
            this.ShoppingLists = new List<ShoppingList>();
        }
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? MobileNumber { get; set; }
        public int? UserRole { get; set; }
        public DateTime LastLoggedInDateTimeStamp { get; set; }

        public string? LastTokenValue { get; set; }
        public DateTime CreatedDateTimeStamp { get; set; }
        public DateTime LastUpdatedDateTimeStamp { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
