using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffStorage
    {
        Type[] GetEventTypes();
        Type GetUpdateType();
        uint GetComponentId();

        void Clear();
        void RemoveEntityComponent(long entityId);

        List<EntityId> GetComponentsAdded();
        List<EntityId> GetComponentsRemoved();
    }

    public interface ICommandDiffStorage
    {
        void Clear();
    }

    public interface IComponentCommandDiffStorage : ICommandDiffStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        Type GetRequestType();
        Type GetResponseType();

        void RemoveRequests(long entityId);
    }

    public interface IDiffComponentAddedStorage<T> where T : ISpatialComponentUpdate
    {
        void AddEntityComponent(long entityId, T component);
    }

    public interface IDiffAuthorityStorage : IComponentDiffStorage
    {
        void AddAuthorityChange(AuthorityChangeReceived authorityChange);
        MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges();
        MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId);
    }

    public interface IDiffUpdateStorage<T> : IComponentDiffStorage
        where T : ISpatialComponentUpdate
    {
        void AddUpdate(ComponentUpdateReceived<T> update);
        MessagesSpan<ComponentUpdateReceived<T>> GetUpdates();
        MessagesSpan<ComponentUpdateReceived<T>> GetUpdates(EntityId entityId);
    }

    public interface IDiffEventStorage<T> : IComponentDiffStorage
        where T : IEvent
    {
        void AddEvent(ComponentEventReceived<T> ev);
        MessagesSpan<ComponentEventReceived<T>> GetEvents();
        MessagesSpan<ComponentEventReceived<T>> GetEvents(EntityId entityId);
    }

    public interface IDiffCommandRequestStorage<T> : ICommandDiffStorage
        where T : struct, IReceivedCommandRequest
    {
        void AddRequest(T request);
        MessagesSpan<T> GetRequests();
        MessagesSpan<T> GetRequests(EntityId targetEntityId);
    }

    public interface IDiffCommandResponseStorage<T> : ICommandDiffStorage
        where T : struct, IReceivedCommandResponse
    {
        void AddResponse(T response);
        MessagesSpan<T> GetResponses();
        MessagesSpan<T> GetResponse(long requestId);
    }
}
