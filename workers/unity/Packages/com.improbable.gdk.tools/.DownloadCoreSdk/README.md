This project exists in folder prefixed with a '.' so that Unity ignores it at script compile time, but it can still be
distributed as part of the Unity package.

Unity doesn't add a project reference to `System.IO.Compression.FileSystem`, which is required for access to the `ZipFile` class.
We want to avoid adding third party dependencies on an external Zip library, so the interim solution is to make this a standalone program for now.
