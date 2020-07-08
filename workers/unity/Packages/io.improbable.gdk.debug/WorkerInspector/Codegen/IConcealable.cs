namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public interface IConcealable<T>
    {
        void SetVisibility(T data, bool hideIfEmpty);
    }
}
