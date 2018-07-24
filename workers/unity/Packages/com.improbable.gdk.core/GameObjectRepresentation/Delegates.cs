using Improbable.Worker;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    // TODO reviewers: shall we put these delegates into their own class/namespace?
    public delegate void AuthorityChangedDelegate(Authority newAuthority);

    public delegate void ComponentUpdateDelegate<in TComponentUpdate>(TComponentUpdate updateData)
        where TComponentUpdate : ISpatialComponentUpdate;
}
