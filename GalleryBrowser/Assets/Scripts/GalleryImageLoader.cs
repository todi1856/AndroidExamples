using UnityEngine;

public class GalleryImageLoader : MonoBehaviour
{
    void Start()
    {

        var rawData = System.IO.File.ReadAllBytes("D:\\80023389-alone-fir-tree-with-a-root-isolated-on-white.jpg"); //"/data/user/0/com.unity.mynativeapp/files/IMG_20241105_232022.jpg");

        Texture2D tex = new Texture2D(2, 2);
        tex.LoadRawTextureData(rawData);
        tex.Apply();

        GetComponent<Renderer>().material.mainTexture = tex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
