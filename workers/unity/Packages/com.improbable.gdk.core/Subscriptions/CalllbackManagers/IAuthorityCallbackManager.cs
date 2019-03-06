namespace Improbable.Gdk.Subscriptions
{
    internal interface IAuthorityCallbackManager : ICallbackManager
    {
        void InvokeLossImminentCallbacks();
    }
}
