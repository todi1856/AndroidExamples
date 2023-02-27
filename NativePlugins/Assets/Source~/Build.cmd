set NDK_ROOT="G:\UnityInstalls\2019.4 .32f1\Editor\Data\PlaybackEngines\AndroidPlayer\NDK"
set PREBUILT_PATH="G:\UnityInstalls\2019.4 .32f1\Editor\Data\PlaybackEngines\AndroidPlayer\NDK\prebuilt\windows-x86_64"
%PREBUILT_PATH%\bin\make.exe -f %NDK_ROOT%\build\core\build-local.mk SHELL=cmd %*
pause