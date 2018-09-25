**Warning:** The [alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](../README.md#recommended-use).

----

# SpatialOS GDK for Unity known issues

Known issue = any major user-facing bug or lack of user-facing feature that:
1. diverges from vanilla Unity ECS design or implementation **OR**
1. diverges from user expectations from a SpatialOS project (e.g. interacting across worker boundaries)

| Issue                                                                                                                                                                                                             | Date added | Unity Ticket | Workaround?                                                           | Fixed? |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------|--------------|-----------------------------------------------------------------------|--------|
| The Unity editor randomly crashes on MacOS when using ECS preview packages. This is not caused by the SpatialOS GDK.                                                                                              | 2018/07/25 | 1064084      | Simply retry.                                                         | No     |
| IL2CPP does not currently work on Mac as it crashes upon starting the game. This is due to a bug in Unity that does not initialize a system correctly, if one of the injected `IComponentData` contains a `long`. | 2018/08/31 | 1076596      | Use the Mono compiler.                                                | Yes    |
| `bool` is not a [blittable type](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types), so will cause an error if used to store component data.                            | 2018/09/17 | None         | Use `BlittableBool` instead.                                          | No     |
| Accessing component data from an entity query will segfault, therefore requests where `ResultType` is `SnapshotResultType` will be dropped before sending.                                                        | 2018/09/19 | None         | Don't send entity queries where `ResultType` is `SnapshotResultType`. | No     |
