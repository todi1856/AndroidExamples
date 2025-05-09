using UnityEngine;

public class AndroidJavaClassExamples : MonoBehaviour
{
    public void Start()
    {
        var integerClass = new AndroidJavaClass("java.lang.Integer");
        int maxValue = integerClass.GetStatic<int>("MAX_VALUE");
        Utilities.Log("java.lang.Integer.MAX_VALUE: " + maxValue);
    }
}
