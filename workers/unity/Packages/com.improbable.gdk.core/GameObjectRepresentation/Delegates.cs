using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;
using Entity = Improbable.Worker.Entity;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    // TODO reviewers: shall we put these delegates into their own class/namespace?
    public delegate void AuthorityChangedDelegate(Authority newAuthority);

    public delegate void ComponentUpdateDelegate<in TComponentUpdate>(TComponentUpdate updateData)
        where TComponentUpdate : ISpatialComponentUpdate;
}
