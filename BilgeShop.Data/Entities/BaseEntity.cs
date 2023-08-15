using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BilgeShop.Data.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

    }

    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == false);
            // Bu veritabanı üzerinde yapılacak bütün sorgulamalarda yukarıdaki linq geçerli olacak. Böylelikle benim silinmemişleri getir diye her defasında ekleme yapmama gerek kalmayacak.


            builder.Property(x => x.ModifiedDate).IsRequired(false);
            // Null olabilir.
        }
    }

    // where TEntity : BaseEntity diyerek, bu class'ın yalnızca BaseEntity tipi yapılarla kullanılabileceğini söylüyorum.


}
