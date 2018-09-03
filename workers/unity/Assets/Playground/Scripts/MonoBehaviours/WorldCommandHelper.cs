using System;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;

public class WorldCommandHelper : IDisposable
{
    private class ContextWrapper<TOp>
    {
        public readonly WorldCommandHelper Helper;
        private readonly Action<TOp> callback;

        public ContextWrapper(WorldCommandHelper worldCommandHelper, Action<TOp> callback)
        {
            Helper = worldCommandHelper;
            this.callback = callback;
        }

        public void Dispatch(TOp op)
        {
            callback(op);
        }
    }

    private readonly WorldCommands.WorldCommandResponseHandler responseHandler;
    private readonly WorldCommands.WorldCommandRequestSender requestSender;

    public WorldCommandHelper(
        WorldCommands.WorldCommandRequestSender requestSender,
        WorldCommands.WorldCommandResponseHandler responseHandler)
    {
        this.requestSender = requestSender;
        this.responseHandler = responseHandler;

        responseHandler.OnReserveEntityIdsResponse += OnReserveEntityIdsResponse;
        responseHandler.OnCreateEntityResponse += OnOnCreateEntityResponse;
        responseHandler.OnDeleteEntityResponse += OnDeleteEntityResponse;
    }

    public void Dispose()
    {
        responseHandler.OnDeleteEntityResponse -= OnDeleteEntityResponse;
        responseHandler.OnCreateEntityResponse -= OnOnCreateEntityResponse;
        responseHandler.OnReserveEntityIdsResponse -= OnReserveEntityIdsResponse;
    }

    private void OnDeleteEntityResponse(WorldCommands.DeleteEntityResponse response)
    {
        if (response.Context is ContextWrapper<DeleteEntityResponseOp> wrapper && wrapper.Helper == this)
        {
            wrapper.Dispatch(response.Op);
        }
    }

    private void OnOnCreateEntityResponse(WorldCommands.CreateEntityResponse response)
    {
        if (response.Context is ContextWrapper<CreateEntityResponseOp> wrapper && wrapper.Helper == this)
        {
            wrapper.Dispatch(response.Op);
        }
    }

    private void OnReserveEntityIdsResponse(WorldCommands.ReserveEntityIdsResponse response)
    {
        if (response.Context is ContextWrapper<ReserveEntityIdsResponseOp> wrapper && wrapper.Helper == this)
        {
            wrapper.Dispatch(response.Op);
        }
    }

    public void ReserveEntityIds(uint numberOfEntityIds, Action<ReserveEntityIdsResponseOp> action)
    {
        requestSender.ReserveEntityIds(numberOfEntityIds, new ContextWrapper<ReserveEntityIdsResponseOp>(this, action));
    }

    public void CreateEntity(Entity entityTemplate, EntityId? entityId, Action<CreateEntityResponseOp> action)
    {
        requestSender.CreateEntity(entityTemplate, entityId, new ContextWrapper<CreateEntityResponseOp>(this, action));
    }

    public void DeleteEntity(EntityId entityId, Action<DeleteEntityResponseOp> action)
    {
        requestSender.DeleteEntity(entityId, new ContextWrapper<DeleteEntityResponseOp>(this, action));
    }
}
