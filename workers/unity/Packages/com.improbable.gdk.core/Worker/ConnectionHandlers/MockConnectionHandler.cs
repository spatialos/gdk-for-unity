using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class MockConnectionHandler : IConnectionHandler
    {
        private uint updateId;

        private readonly ViewDiff diff = new ViewDiff();

        public void ClearDiff()
        {
            diff.Clear();
        }

        public void CreateEntity(long entityId, EntityTemplate template)
        {
            var handler = new EntityTemplateDynamicHandler(template, entityId, diff);
            DynamicConverter.ForEachComponent(handler);
        }

        public void ChangeAuthority(long entityId, uint componentId, Authority newAuthority)
        {
            diff.SetAuthority(entityId, componentId, newAuthority);
        }

        public void UpdateComponent<T>(long entityId, uint componentId, T update) where T : ISpatialComponentUpdate
        {
            diff.AddComponentUpdate(update, entityId, componentId, updateId++);
        }

        public void AddEvent<T>(long entityId, uint componentId, T ev) where T : IEvent
        {
            diff.AddEvent(ev, entityId, componentId, updateId++);
        }

        // TODO: Commands

        #region IConnectionHandler implementation

        public ViewDiff GetMessagesReceived()
        {
            return diff;
        }

        public void PushMessagesToSend(MessagesToSend messages)
        {
            throw new System.NotImplementedException();
        }

        public bool IsConnected()
        {
            return true;
        }

        #endregion

        private class EntityTemplateDynamicHandler : DynamicConverter.IConverterHandler
        {
            private EntityTemplate template;
            private ViewDiff viewDiff;
            private long entityId;

            public EntityTemplateDynamicHandler(EntityTemplate template, long entityId, ViewDiff viewDiff)
            {
                this.template = template;
                this.viewDiff = viewDiff;
                this.entityId = entityId;

                viewDiff.AddEntity(this.entityId);
            }

            public void Accept<TSnapshot, TUpdate>(uint componentId,
                DynamicConverter.SnapshotToUpdate<TSnapshot, TUpdate> snapshotToUpdate)
                where TSnapshot : struct, ISpatialComponentSnapshot
                where TUpdate : struct, ISpatialComponentUpdate
            {
                var maybeSnapshot = template.GetComponent<TSnapshot>();

                if (!maybeSnapshot.HasValue)
                {
                    return;
                }

                var snapshot = maybeSnapshot.Value;
                viewDiff.AddComponent(snapshotToUpdate(snapshot), entityId, componentId);
            }
        }
    }
}
