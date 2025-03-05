﻿using EventManagment.Core.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagment.Infrastructure.Persistence._Data.Configuration.Base
{
    internal class BaseAuditableEntityConfigurations<TEntity, TKy> : BaseEntityConfigurations<TEntity, TKy>
         where TEntity : BaseAuditableEntity<TKy> where TKy : IEquatable<TKy>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.Property(E => E.CreatedBy).IsRequired();

            builder.Property(E => E.CreatedOn).IsRequired();


            builder.Property(E => E.LastModifiedBy).IsRequired();

            builder.Property(E => E.LastModifiedOn).IsRequired();
        }
    }
}
