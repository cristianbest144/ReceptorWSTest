using System;

namespace Shared.Network.MapperModel
{
    public interface IMapperModel
    {
        Type GetModelType();
        Type GetEntityType();
        Type GetKeyType();
    }
}