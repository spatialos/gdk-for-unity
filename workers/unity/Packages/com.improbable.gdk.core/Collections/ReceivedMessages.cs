namespace Improbable.Gdk.Core
{
    public interface IReceivedMessage
    {
    }

    public interface IReceivedEntityMessage : IReceivedMessage
    {
        EntityId GetEntityId();
    }
}
