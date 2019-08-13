![](spatialos_gdk_for_unity_header.png)

![](https://img.shields.io/badge/license-MIT-brightgreen.svg) ![](https://badge.buildkite.com/fec962a4df6e6705871bffa4dfcdea4f2ff7efcd737e5186ea.svg?branch=develop) ![](https://img.shields.io/github/release/spatialos/gdk-for-unity.svg)

The SpatialOS Game Development Kit (GDK) for Unity allows you to quickly and easily build and host Unity multiplayer games. These games can use multiple server-side game engines across one seamless world to create new kinds of gameplay.

If you're an Unity game developer and you're ready to try out the GDK, follow the [Get started guide](https://docs.improbable.io/unity/alpha/projects/fps/get-started/get-started).

> The SpatialOS GDK for Unity is in alpha. We are committed to rapid development of the GDK to improve stability, usability, and performance - for information on this, see our [development roadmap](https://github.com/spatialos/gdk-for-unity/projects/1) and contact us via our [forums](https://forums.improbable.io/latest?tags=unity-gdk), or on [Discord](https://discord.gg/SCZTCYm). Sign onto our [mailing list](http://go.pardot.com/l/169082/2018-06-25/27mhsb) to get updates about the GDK for Unity.


## Anatomy of the GDK

The GDK is composed of three layers:

**The GDK Core:** a performant, data-oriented integration with our cloud platform SpatialOS, based on the familiar Unity-native workflows.

**The GDK Feature Modules:** a library of solutions for hard or common networked game development problems, such as Character Movement and Shooting.

**The GDK Starter Projects:**

* The [**First Person Shooter (FPS) Starter Project**](https://github.com/spatialos/gdk-for-unity-fps-starter-project/)

  This project enables you and your friends to experience the true scale of SpatialOS, providing a solid foundation for entirely new games in the FPS genre.
  
* The [**Blank Starter Project**](https://github.com/spatialos/gdk-for-unity-blank-project) 

  This project contains the minimum GDK feature set you need to start developing games for SpatialOS.

## Developing on the GDK

We recommend that you start developing with the [**Blank Starter Project**](https://github.com/spatialos/gdk-for-unity-blank-project) or the [**First Person Shooter (FPS) Starter Project**](https://github.com/spatialos/gdk-for-unity-fps-starter-project/).

If you'd like to develop on this repository, there is a little bit of additional setup:

### Developer dependencies

In addition to the dependencies listed [here](https://docs.improbable.io/unity/alpha/machine-setup#4-install-the-gdk-dependencies), you will need the following programs present on your `PATH`:

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

## Having problems?

* [Documentation](https://docs.improbable.io/unity/alpha/)
* [Troubleshooting](https://docs.improbable.io/unity/alpha/reference/troubleshooting)
* [Known issues](https://github.com/spatialos/gdk-for-unity/projects/2)

If you are unable to resolve your issue, please raise an issue [in this repository](https://github.com/spatialos/UnityGDK/issues) or find us on the [forums](https://forums.improbable.io/latest?tags=unity-gdk) or [Discord](https://discord.gg/SCZTCYm).

## Recommended use

This is the alpha release of the SpatialOS GDK for Unity. We invite projects to start using it but warn that all APIs are subject to change as we learn from feedback.

## Give us feedback

We have released the GDK this early in development because we want your feedback. Please come and talk to us about the software and the documentation via:

* **Discord**
  
  Find us in the [**#unity** channel](https://discord.gg/SCZTCYm). You may need to grab Discord [here](https://discordapp.com).

* **The SpatialOS forums**

  Visit the **feedback** section in our [forums](https://forums.improbable.io) and use the `unity-gdk` tag. [This link](https://forums.improbable.io/new-topic?category=Feedback&tags=unity-gdk) takes you there and pre-fills the category and tag.

* **Github issues**

  Create an issue [in this repository](https://github.com/spatialos/UnityGDK/issues).

## Contributions

We are not currently accepting public contributions - see our [contributions](https://docs.improbable.io/unity/alpha/contributing) policy. However, we are accepting issues and we do want your feedback.

---

* Version: [alpha](https://docs.improbable.io/reference/latest/shared/release-policy)
* Your access to and use of the Unity Engine is governed by the Unity Engine End User License Agreement. Please ensure that you have agreed to those terms before you access or use the Unity Engine.

&copy; 2019 Improbable
