using System;
using System.Text;
using UnityEngine;

public class PassingParameters : MonoBehaviour
{
    void Start()
    {
        using var o = new AndroidJavaObject("com.unity3d.player.PassingParameters");

        Utilities.Log($"From Java Byte: " + o.Call<sbyte>("getByte"));
        Utilities.Log($"From Java Short: " + o.Call<short>("getShort"));
        Utilities.Log($"From Java Int: " + o.Call<int>("getInt"));
        Utilities.Log($"From Java Long: " + o.Call<long>("getLong"));
        Utilities.Log($"From Java Float: " + o.Call<float>("getFloat"));
        Utilities.Log($"From Java Double: " + o.Call<double>("getDouble"));
        Utilities.Log($"From Java Boolean: " + o.Call<bool>("getBoolean"));
        Utilities.Log($"From Java Char: " + o.Call<char>("getChar"));
        Utilities.Log($"From Java String: " + o.Call<string>("getString"));

        o.Call("setByte", (sbyte)123);
        o.Call("setShort", (short)245);
        o.Call("setInt", 567);
        o.Call("setLong", (long)111);
        o.Call("setFloat", 1.23f);
        o.Call("setDouble", 2.456);
        o.Call("setBoolean", true);
        o.Call("setChar", 'z');
        o.Call("setString", "Hello from C#");


        var complexJson = o.Call<String>("getComplexStructure");
        var c = JsonUtility.FromJson<ComplexStructure>(complexJson);
        Utilities.Log("From Java ComplexStructure:");
        Utilities.Log(" " + c.ToString());

        c = new ComplexStructure((byte)11, (short)21, 31, 41L, 51.5f, 61.6d, true, "Hello C#!");
        o.Call("setComplexStructure", JsonUtility.ToJson(c));
    }
}
