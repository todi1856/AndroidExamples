using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class RegisterNatives : MonoBehaviour
{
    AndroidJavaObject m_JavaObject;

    void Start()
    {
        m_JavaObject = new AndroidJavaObject("com.unity3d.player.RegisterNativesTest");
        var callback = new MyCallbackDelegate(MyManagedCallback);
        var funcPtr = Marshal.GetFunctionPointerForDelegate(callback);

        // Keep a reference to the delegate to prevent GC
        GC.KeepAlive(callback);

        var nativeMethods = new[]
        {
            new JNINativeMethod()
            {
                name = "myManagedCallback",
                signature = "(II)I",
                fnPtr = funcPtr
            }
        };

        AndroidJNI.RegisterNatives(m_JavaObject.GetRawClass(), nativeMethods);
        m_JavaObject.CallStatic("callNativeMethod");
    }

    public delegate int MyCallbackDelegate(IntPtr jniEnv, IntPtr klass, int x, int y);

    [MonoPInvokeCallback(typeof(MyCallbackDelegate))]
    static int MyManagedCallback(IntPtr jniEnv, IntPtr klass, int a, int b)
    {
        Utilities.Log($"Callback invoked with: {a}, {b}");
        return a + b;
    }
}
