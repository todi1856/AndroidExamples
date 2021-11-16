using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SendMessageReceiver : MonoBehaviour
{
    ListView listView;
    List<string> items;

    public void Start()
    {
        var doc = GetComponent<UIDocument>();
        items = new List<string>();

        var label = new Label("Tap on screen to trigger C++ -> C# send message.");
        label.style.color = Color.white;
        label.style.fontSize = 13;
        doc.rootVisualElement.Add(label);

        listView = new ListView();
        listView.itemsSource = items;
        listView.style.color = Color.white;
        listView.style.fontSize = 13;
        doc.rootVisualElement.Add(listView);
    }

    void AddItem(string message)
    {
        items.Insert(0, message);
        if (items.Count > 10)
            items.RemoveAt(items.Count - 1);
        listView.RefreshItems();
    }

    public void SendMessageFromCpp(string message)
    {
        AddItem(message);
        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
    }
}
