using UnityEngine;

public class KotlinExamples : MonoBehaviour
{
    void Start()
    {
        using var helper = new AndroidJavaObject("KotlinStringHelper");
        var value = helper.Call<string>("getString");
        Utilities.Log($"KotlinStringHelper.getString returns {value}");
    }
}

