using UnityEngine;
using UnityEngine.Android;

public class GalleryImageLoader : MonoBehaviour
{
    // Called from Java
    public void ImageDataLoaded()
    {
        var rawData = AndroidApplication.currentActivity.Call<byte[]>("getImageData");
        LoadImageBytes(rawData);
        AndroidApplication.currentActivity.Call<byte[]>("clearImageData");
    }

    private void LoadImageBytes(byte[] data)
    {
        var tex = new Texture2D(2, 2);
        ImageConversion.LoadImage(tex, data, true);

        GetComponent<Renderer>().material.mainTexture = tex;
    }
}
