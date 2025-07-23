package com.unity3d.player;

import android.util.Log;


public class RegisterNativesTest
{
    public static void callNativeMethod() {
        Utilities.log("Calling native method");

        var result = myManagedCallback(4, 3);
        Utilities.log("4 + 3 = " + result);
    }

    static native int myManagedCallback(int a, int b);
}