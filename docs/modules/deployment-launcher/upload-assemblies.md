<%( TOC )%>

# Upload assemblies

Open the Deployment Launcher window by selecting **SpatialOS** > **Deployment Launcher** from the Unity Editor menu.

## UI walkthrough

The section of the Deployment Launcher window that you use to upload assemblies can be found under the **Assembly Upload** label:

<img src="{{assetRoot}}assets/modules/deployment-launcher/assembly-upload.png" style="margin: 0 auto; width: 100%; display: block;" />

There are two settings that you can edit:

| Setting | Description |
| --- | --- |
| Assembly Name | This is an identifier for the assembly you will upload.<br/><br/>You can use this to reference an assembly when launching a deployment or to find the assembly in the SpatialOS Console. |
| Force Upload | Denotes whether to force upload this assembly.<br/><br/>If this is checked, an assembly that previously was uploaded with the same assembly name will be overwritten. |

There are also three buttons:

| Button name | Description |
| --- | --- |
| Generate | When pressed, this generates an **Assembly Name** for you based on your SpatialOS project name and a timestamp. |
| Assign assembly name to deployments | When pressed, this copies the current value of **Assembly Name** to _all_ deployments you currently have [configured in the next section]({{urlRoot}}/modules/deployment-launcher/launch-deployments). |
| Upload assembly | When pressed, this initiates the assembly upload process. |

## Expected behaviour

### Input validation

The Deployment Launcher will validate your chosen assembly name to ensure that it is properly formed. Your assembly name must:

* Must contain at least 5 characters.
* Must contain at most 64 characters.
* Not contain invalid characters.
  * `_`, `.`, and `-` are accepted characters.

If validation fails, the **Upload assembly** button will be disabled and you will see an error similar to below:

```text
Assembly Name "<invalid_name>" is invalid. Must conform to the regex: ^[a-zA-Z0-9_.-]{5,64}
```

### The upload process

When you press the upload assembly button, the Deployment Launcher starts a `spatial cloud upload` process under the hood. You should see a notification similar to the following at the bottom of the deployment launcher window:

```text
Uploading assembly "<assembly_name>".
Assembly reloading locked.
```

The standard output and standard error from the command is forwarded to the Unity Console.

<%(Callout message="
The assembly has finished uploading when:

* You see the following message in your Unity Editor Console window: `Upload of <assembly_name> succeeded.`
* The notification at the bottom of the Deployment Launcher window has disappeared.
")%>
