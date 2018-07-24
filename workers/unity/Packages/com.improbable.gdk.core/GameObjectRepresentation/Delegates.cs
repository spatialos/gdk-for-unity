using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    // TODO reviewers: shall we put these delegates into their own class/namespace?
    public delegate void AuthorityChangedDelegate(Authority newAuthority);

    public delegate void ComponentUpdateDelegate<TComponentUpdate>(TComponentUpdate updateData)
        where TComponentUpdate : ISpatialComponentUpdate;
}
