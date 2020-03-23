using System.Runtime.CompilerServices;
using Improbable.Gdk.GameObjectCreation;
using Unity.Entities;

[assembly: InternalsVisibleTo("Improbable.Gdk.Core.EditmodeTests")]
[assembly: InternalsVisibleTo("Improbable.Gdk.GameObjectCreation.EditmodeTests")]
[assembly: InternalsVisibleTo("Improbable.Gdk.EditmodeTests")]
[assembly: InternalsVisibleTo("Improbable.Gdk.PlaymodeTests")]

[assembly: RegisterGenericComponentType(typeof(GameObjectInitSystemStateComponent))]
