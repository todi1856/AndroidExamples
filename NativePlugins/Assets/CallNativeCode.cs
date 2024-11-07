using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;
using System;

public class CallNativeCode : MonoBehaviour {

	[DllImport("native")]
	private static extern float add(float x, float y);

    private void Start()
    {
        var x = 3;
        var y = 10;

        var result = string.Empty;
        try
        {
            result = add(x, y).ToString();
        }
        catch (Exception ex)
        {
            result = ex.ToString();
        }
#if UNITY_EDITOR
        result = "Run me on Android";
#endif
        var doc = GetComponent<UIDocument>();
        var l = new Label($"Result is = {result}");
        l.style.color = Color.white;
        l.style.fontSize = 20;
        doc.rootVisualElement.Add(l);
    }
}
