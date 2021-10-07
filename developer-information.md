![](spatialos_gdk_for_unity_header.png)

## Developing on the GDK

We recommend that you start developing with the [**Blank Starter Project**](https://github.com/spatialos/gdk-for-unity-blank-project) or the [**First Person Shooter (FPS) Starter Project**](https://github.com/spatialos/gdk-for-unity-fps-starter-project/).

If you'd like to develop on this repository, there is a little bit of additional setup:

### Developer dependencies

In addition to the dependencies listed [here](https://documentation.improbable.io/gdk-for-unity/docs/setup-and-dependencies#section-4-install-the-gdk-dependencies), you will need the following programs present on your `PATH`:

* [`jq`](https://stedolan.github.io/jq/)

### Setup

1. Checkout this repository.
2. Run `init.sh` or `init.ps1` to download the dependencies.
3. Open the Unity project in `workers/unity`.

> **Why do I need to hit init?**
> 
> As of [release 0.2.5](https://github.com/spatialos/gdk-for-unity/releases/tag/0.2.5), we publish the dependencies as UPM (NPM) packages. This repository contains the source for these packages, but dependencies such as the Worker SDK and schema standard library are not committed to the repository. `init.sh`/`init.ps1` downloads these dependencies and puts them in the correct place.
>
> If you fail to run `init.sh`/`init.ps1` before opening Unity, Unity _will_ delete the `.meta` files associated with the files downloaded by `init.sh`/`init.ps1`. If this happens to you, run `git reset --hard` to reset the repostory back to a good state. Note that this is a destructive `git` operation, so any un-committed changes will be lost. 
