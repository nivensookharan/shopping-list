using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingList.Dal.Mapping
{
    internal class ShoppingListMap : IEntityTypeConfiguration<Domain.ShoppingList>
    {
        public void Configure(EntityTypeBuilder<Domain.ShoppingList> builder)
        {
            // Primary Keys
            builder.HasKey(x => x.ShoppingListId);

            builder.HasMany(r => r.ShoppingListItems)
                .WithOne(x => x.ShoppingList).HasForeignKey(f => f.ShoppingListId);
                
                



            // Table
            builder.ToTable("ShoppingList");
        }
    }
}
