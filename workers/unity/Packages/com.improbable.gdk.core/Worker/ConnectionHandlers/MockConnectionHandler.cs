using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class MockConnectionHandler : IConnectionHandler
    {
        private uint updateId;

        private ViewDiff currentDiff => diffs[currentDiffIndex];

        private readonly ViewDiff[] diffs = new[]
        {
            new ViewDiff(),
            new ViewDiff()
        };

        private int currentDiffIndex = 0;

        public void CreateEntity(long entityId, EntityTemplate template)
        {
            var handler = new EntityTemplateDynamicHandler(template, entityId, currentDiff);
            DynamicConverter.ForEachComponent(handler);
        }

        public void ChangeAuthority(long entityId, uint componentId, Authority newAuthority)
        {
            currentDiff.SetAuthority(entityId, componentId, newAuthority);
        }

        public void UpdateComponent<T>(long entityId, uint componentId, T update) where T : ISpatialComponentUpdate
        {
            currentDiff.AddComponentUpdate(update, entityId, componentId, updateId++);
        }

        public void AddEvent<T>(long entityId, uint componentId, T ev) where T : IEvent
        {
            currentDiff.AddEvent(ev, entityId, componentId, updateId++);
        }

        public void UpdateComponentAndAddEvents<TUpdate, TEvent>(long entityId, uint componentId, TUpdate update,
            params TEvent[] events)
            where TUpdate : ISpatialComponentUpdate
            where TEvent : IEvent
        {
            var thisUpdateId = updateId++;

            currentDiff.AddComponentUpdate(update, entityId, componentId, thisUpdateId);
            foreach (var ev in events)
            {
                currentDiff.AddEvent(ev, entityId, componentId, thisUpdateId);
            }
        }

        // TODO: Commands

        #region IConnectionHandler implementation

        public void GetMessagesReceived(ref ViewDiff viewDiff)
        {
            var diffToReturn = currentDiff;

            currentDiffIndex = (currentDiffIndex + 1) % diffs.Length;
            var nextDiff = currentDiff;
            nextDiff.Clear();

            viewDiff = diffToReturn;
        }

        public MessagesToSend GetMessagesToSendContainer()
        {
            return new MessagesToSend();
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

        public void Dispose()
        {
        }
    }
}
