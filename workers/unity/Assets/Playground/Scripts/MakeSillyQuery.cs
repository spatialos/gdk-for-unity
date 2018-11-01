using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker.Query;
using UnityEngine;

[WorkerType("UnityClient")]
public class MakeSillyQuery : MonoBehaviour
{
    [Require] private WorldCommands.Requirable.WorldCommandRequestSender sender;
    [Require] private WorldCommands.Requirable.WorldCommandResponseHandler handler;

    private void OnEnable()
    {
        var nearTheOriginConstraint = new AndConstraint(new IConstraint[]
        {
            new ComponentConstraint(Position.ComponentId),
            new SphereConstraint(0.0, 0.0, 0.0, 50.0)
        });

        var query = new EntityQuery
        {
            ResultType = new SnapshotResultType(new List<uint> { Position.ComponentId }),
            Constraint = nearTheOriginConstraint
        };
        sender.EntityQuery(query);

        handler.OnEntityQueryResponse += HandleQuery;
    }

    private void HandleQuery(WorldCommands.EntityQuery.ReceivedResponse response)
    {
        foreach (var entity in response.Result)
        {
            Debug.Log(entity.Value.GetComponentSnapshot<Position.Snapshot>().Value.Coords.X);
        }
    }
}
