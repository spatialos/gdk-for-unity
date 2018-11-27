using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class AuthorityComparer : IComparer<AuthorityChangeReceived>
    {
        public int Compare(AuthorityChangeReceived x, AuthorityChangeReceived y)
        {
            return x.EntityId.Id.CompareTo(y.EntityId.Id);
        }
    }
}
