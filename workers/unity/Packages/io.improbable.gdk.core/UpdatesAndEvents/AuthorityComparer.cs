using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class AuthorityComparer : IComparer<AuthorityChangeReceived>
    {
        public int Compare(AuthorityChangeReceived x, AuthorityChangeReceived y)
        {
            var entityIdCompare = x.EntityId.Id.CompareTo(y.EntityId.Id);

            if (entityIdCompare == 0)
            {
                return x.AuthorityChangeId.CompareTo(y.AuthorityChangeId);
            }

            return entityIdCompare;
        }
    }
}
