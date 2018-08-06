**Warning:** The [pre-alpha](https://docs.improbable.io/reference/latest/shared/release-policy#maturity-stages) release is for evaluation purposes only, with limited documentation - see the guidance on [Recommended use](README.md#recommended-use).

----

# Unity GDK known issues

Known issue = any major user-facing bug or lack of user-facing feature that:
1. diverges from vanilla Unity ECS design or implementation **OR**
1. diverges from user expectations from a SpatialOS project (e.g. interacting across worker boundaries)

| Issue | Date added | Ticket | Workaround? | Done? |
|-------|-------------------|--------|-------------|-------|
| The Unity editor randomly crashes on MacOS when using ECS preview packages. | 2018-07-25 | [1064084](https://fogbugz.unity3d.com/default.asp?1064084_q5lp4g8hn0vhp706) | Simply retry. | No|