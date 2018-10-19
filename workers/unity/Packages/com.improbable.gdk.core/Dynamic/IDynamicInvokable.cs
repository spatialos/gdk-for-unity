namespace Improbable.Gdk.Core
{
    public interface IDynamicInvokable
    {
        uint ComponentId { get; }
        void InvokeHandler(Dynamic.IHandler handler);
    }
}
