<%(TOC)%>

# How to add a schema source directory

By default, the GDK finds and compiles schema found within `.schema/` folders of packages referenced in your project's `manifest.json`.

To compile schema from directories outside of referenced packages, you must add your schema source directories to the GDK tools configuration.

##### 1. Create a folder for schema

First, create the folder in which you will be writing schema files. We recommend adding a `schema/` folder at the root of your SpatialOS project.

```text
  <project_root>
    ├── schema/
    ├── snapshots/
    ├── workers/
```

##### 2. Open GDK tools configuration in Unity Editor

In the Unity Editor, select **SpatialOS** > **GDK tools configuration** to open the GDK tools configuration window.

<img src="{{assetRoot}}assets/blank/schema/select-tools-config.png" style="margin: 0 auto; width: auto; display: block;" />

##### 3. Add a schema source directory

If you are setting up the blank project for the first time, you will find that there are no directories listed under the _Schema sources_ section.

<img src="{{assetRoot}}assets/blank/schema/add-schema-source-before.png" style="margin: 0 auto; width: 50%; display: block;" />

To add an entry, select the ➕ icon and enter the path to the schema directory you created earlier. **Note that the path must be relative to your Unity project directory.**

> The GDK tools configuration window will display an error if the source directory does not exist.

<img src="{{assetRoot}}assets/blank/schema/add-schema-source-after.png" style="margin: 0 auto; width: 50%; display: block;" />

After adding this entry and ensuring that no errors are displayed, select **Save** and close the window.

<%(#Expandable title="What should <code>GdkToolsConfiguration.json</code> look like when I'm done?")%>

```json
{
    "SchemaSourceDirs": [
        "../../schema"
    ],
    "CodegenOutputDir": "Assets/Generated/Source",
    "DescriptorOutputDir": "../../build/assembly/schema",
    "DevAuthTokenDir": "Resources",
    "DevAuthTokenLifetimeDays": 30,
    "SaveDevAuthTokenToFile": false
}
```

<%(/Expandable)%>
