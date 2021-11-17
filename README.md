# Android Examples

## Native Plugins

Shows how to compile native c/cpp files into a shared library for different architectures and use it from C# script

Quick Steps:
* Install Unity with Android Support (I used 2019.4.32)
	* Note: Google tools have a limitation that NDK path shouldn't contain any whitespaces. Either install Unity into a path with no whi
* Open the project 
* NativePluginBuilder window should open, if it's not opened, from main menu go to Plugins->Build.
* Select desired architectures, for ex., ARMv7 and ARM64
* Click Build Native Plugins
* If all good, it should say **Result: Success**, the native plugins should be copied to Assets/Plugins/[abi], and appropriate plugin settings (like CPU) should be set
* Select Il2Cpp in Player Settings
* Pick the desired Android Architectures from Player Settings
* Build & Run
* If all good, on Android device you should see 

		adding 3 and 10 in native code equals 13
