namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandSerializer
    {
        void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages,
            CommandMetaData commandMetaData);
    }
}
