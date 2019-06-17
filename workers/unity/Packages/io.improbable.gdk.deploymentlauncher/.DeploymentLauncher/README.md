This project exists in a folder prefixed with a '.' so that Unity ignores it at script compile time, but it can still be
distributed as part of the Unity package.

Due to the fact that the deployment launcher depends on the Platform SDK, and the Platform SDK needs
to be installed by NuGet (as it pulls in a number of dependencies), it was easiest to move it to
a standalone project.
