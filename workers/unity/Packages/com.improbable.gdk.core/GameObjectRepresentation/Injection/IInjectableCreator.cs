using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Interface for objects creating a particular type of IInjectable, to be used by the InjectableFactory.
    /// </summary>
    public interface IInjectableCreator
    {
        IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher);
    }
}
