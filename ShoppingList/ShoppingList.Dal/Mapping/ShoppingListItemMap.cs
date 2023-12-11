using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingList.Domain;

namespace ShoppingList.Dal.Mapping
{
    public class ShoppingListItemMap: IEntityTypeConfiguration<ShoppingListItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
        {
            // Primary Keys
            builder.HasKey(x => x.ShoppingListItemId);


            builder.Property(x => x.ImageUrl).IsRequired(false);
            // Foreign Keys

            // Properties



            // Table
            builder.ToTable("ShoppingListItem");
        }
    }
}
