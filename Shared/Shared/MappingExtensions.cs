using Shared.Network.MapperModel;
using System;
using System.Linq.Expressions;

namespace Shared.Shared
{
    public static class MappingExtensions
    {
        public static void Map(this object reference, object entity)
        {
            MappingHelper.MapEntity(entity, reference);
        }

        public static void Map<T>(this T reference, object entity, Expression<Func<T, object>> columns)
        {
            MappingHelper.MapEntity<T>(entity, reference, columns);
        }

        public static E MapToEntity<E>(this object model)
        {
            return MappingHelper.MapModelToEntity<E>(model);
        }
    }
}
