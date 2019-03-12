<%(TOC)%>
# SpatialOS GDK for Unity known issues


Something in the GDK not working as expected? These are the known issues; later releases of the GDK may contain fixes for these.  
If your issue is not listed here, you can raise an issue through our [GitHub repo](https://github.com/spatialos/unrealgdk/issues).

| Issue                                                                                                                                                                                                                                                                                                                        | Date added | Unity Ticket                                                                                           | Workaround?                                                           |
|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------|--------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------|
| Running the FPS Starter Project in the Unity Editor with the **current platform** set as Android leads to the walls being slightly transparent.                                                                                                      | 2019/01/28 | None                                                                                                | It renders correctly when running in the Emulator or an Android device.                                                |
| Building a worker in the FPS Starter Project for Linux throws errors when compiling the `Hidden/PostProcessing/FinalPass` shader.                                                                                                    | 2019/01/28 | None                                                                                                | The build still succeeds and won't affect your worker.                                                |

