using UnityEngine;

public class AndroidJavaClassExamples : MonoBehaviour
{
    public void Start()
    {
        using var systemClass = new AndroidJavaClass("java.lang.System");
        var hashCode = systemClass.CallStatic<int>("identityHashCode");
        Utilities.Log($"java.lang.System.identityHashCode returns {hashCode}");
    }
}
