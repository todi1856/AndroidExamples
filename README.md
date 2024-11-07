# Android Examples

## PluginResUpgrader
* Starting with Unity 2021.2, if you have Assets/Plugins/Android/res folder you'll get an error:

 **OBSOLETE - Providing Android resources in Assets/Plugins/Android/res was removed, please move your resources to an AAR or an Android Library. See "AAR plug-ins and Android Libraries" section of the Manual for more details.**

 * The project provide a simple upgrader for this issue, if you using .aar or .androidlib plugins is not an option
 * Simply move **PluginResUpgrader/Assets/Editor/AndroidResUpgrader.cs** into your project Assets/Editor folder.
 * Restart Editor
 * Dialog will show up asking if you want to upgrade, upon choosing Yes:
     * Unity will copy Assets/Plugins/Android/res into Assets/Plugins/Android/res-legacy
     * Upon building to Android, the script will manually copy contents of Assets/Plugins/Android/res-legacy to <gradle_project>unityLibrary/src/main/res folder.



| **Name**    | **Description** | **Image** | **Unity Version** |
| :--- | :--- | :--- | :--- |
| [Native Plugin Builder](/Docs/NativePluginBuilder.md) | Shows how to compile native c/cpp files into a shared library (libnative.so) for different architectures using ndk-build and use that native shared library from C# script. | ![](Docs/Images/native_plugin_builder.png) | Unity 6.0 |
| [Gallery Browser](/Docs/GalleryBrowser.md) | Shows how to access an image from phone's gallery via Android native UI and pass it to Unity.<br>**Note:** Unity view only occupies part of the application window. |  ![](Docs/Images/gallery_browser_index.png) | Unity 6.0 |
