# SpatialOS GDK for Unity known issues

A known issue is any major user-facing bug or lack of user-facing feature that:

* Diverges from vanilla Unity ECS design or implementation. 
<br/>**OR**
* Diverges from user expectations of a SpatialOS project (for example; interacting across worker instance boundaries).

| Issue                                                                                                                                                                                                                                                                                                                        | Date added | Unity Ticket                                                                                           | Workaround?                                                           |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------|--------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------|
| The Unity Editor doesn't correctly import NavMeshes.                                                                                                                                                                                                                                                                         | 2018/10/12 | None                                                                                                   | Manually regenerate the NavMesh after opening your project.            |
| Running the FPS Starter Project in the Unity Editor with the current platform as Android leads to the walls being slightly transparent.                                                                                                      | 2019/01/28 | None                                                                                                | It renders correctly when running in the Emulator or an Android device                                                |
| Building a worker in the FPS Starter Project for Linux will throw errors when compiling the `Hidden/PostProcessing/FinalPass` shader.                                                                                                    | 2019/01/28 | None                                                                                                | The build still succeeds and won't affect your worker.                                                |

