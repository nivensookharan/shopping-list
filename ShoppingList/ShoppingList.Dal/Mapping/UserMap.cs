using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShoppingList.Dal.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Keys
            builder.HasKey(x => x.UserId);

            // Foreign Keys

            // Properties

            builder.HasMany(r => r.ShoppingLists)
                .WithOne(x => x.User)
                .HasForeignKey(f => f.UserId);


            // Table
            builder.ToTable("User");
        }
    }
}

