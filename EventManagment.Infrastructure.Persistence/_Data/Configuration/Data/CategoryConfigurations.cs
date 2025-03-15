using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Infrastructure.Persistence._Data.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Data
{
    internal class CategoryConfigurations : BaseAuditableEntityConfigurations<Category, int>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {



            base.Configure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);



            builder.Property(p => p.Name)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);
        }
    }
}
