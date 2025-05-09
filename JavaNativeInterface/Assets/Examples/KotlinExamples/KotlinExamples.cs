using UnityEngine;

public class KotlinExamples : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        using var kotlinClass = new AndroidJavaClass("KotlinStringHelper");
        var value = kotlinClass.CallStatic<string>("getString");
        Utilities.Log($"KotlinStringHelper.getString returns {value}");
    }
}

