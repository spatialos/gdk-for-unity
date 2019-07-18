# Build System Feature Module

This feature module provides tooling for building your GDK for Unity workers inside the Unity Editor.

> **Note:** The Build System Feature Module does not have an integration for building non-Unity workers at this time.

## Installation

Add this feature module to your project via the [Package Manager UI](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.1/manual/index.html#installing-a-new-package).

Or add it to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "io.improbable.gdk.buildsystem": "<%(Var key="current_version")%>"
  },
}
```
