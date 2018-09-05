using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Interface for objects creating a particular type of IInjectable, to be used by the InjectableFactory.
    /// </summary>
    public interface IInjectableCreator
    {
        IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher);
    }
}
