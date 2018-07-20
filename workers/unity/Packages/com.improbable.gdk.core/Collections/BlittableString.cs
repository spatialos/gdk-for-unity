namespace Improbable.Gdk.Core.Collections
{
    public struct BlittableString
    {
        private readonly uint handle;

        internal string Internal => StringProvider.Get(handle);

        internal BlittableString(uint handle)
        {
            this.handle = handle;
        }

        public void Set(string value)
        {
            StringProvider.Set(handle, value);
        }

        public static implicit operator string(BlittableString bl)
        {
            return bl.Internal;
        }
    }
}
