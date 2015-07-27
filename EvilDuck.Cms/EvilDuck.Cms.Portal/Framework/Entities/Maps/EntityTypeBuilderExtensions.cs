using EvilDuck.Cms.Portal.Framework.Utils;
using Microsoft.Data.Entity.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Entities.Maps
{
    public static class EntityTypeBuilderExtensions
    {

        public static void MapStandardEntityFields<TEntity>(this EntityTypeBuilder builder) where TEntity : Entity
        {
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.Version)).ConcurrencyToken();

            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.CreatedBy)).Required().MaxLength(MappingConstants.UsernameLength);
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.CreatedOn)).Required();
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.IsActive)).Required();
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.LastUpdatedBy)).Required().MaxLength(MappingConstants.UsernameLength);
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.LastUpdatedOn)).Required();
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.Reference));
        }

        public static void MapStandardEntityIntKey<TEntity>(this EntityTypeBuilder builder) where TEntity : Entity<int>
        {
            builder.Key(Pn<TEntity>.Get(e => e.Id));
            builder.Property<TEntity>(Pn<TEntity>.Get(e => e.Id)).GenerateValueOnAdd();
        }

    }

    public class MappingConstants
    {

        public const int UsernameLength = 64;

    }
}
