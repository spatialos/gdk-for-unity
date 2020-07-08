namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public interface IConcealable<T>
    {
        void SetVisibility(T dataSource, bool hideIfEmpty);
    }
}
