namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public interface IConcealable
    {
        void HandleSettingChange(HideCollectionEvent evt);

        void SetVisibility(bool isHidden);
    }
}
