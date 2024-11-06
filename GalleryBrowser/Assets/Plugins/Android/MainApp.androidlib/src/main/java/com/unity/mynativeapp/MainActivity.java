package com.unity.mynativeapp;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.Toast;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class MainActivity extends UnityPlayerActivity {
    final int SelectImage = 1234;
    byte[] m_ImageData;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_main);

        LinearLayout contents = findViewById(R.id.contents);
        contents.addView(mUnityPlayer.getFrameLayout());
    }

    public void onOpenGallery(View view) {
        Intent intent = new Intent();
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        startActivityForResult(Intent.createChooser(intent, "Select Picture"), SelectImage);
    }

    private byte[] getBytes(InputStream inputStream) throws IOException {
        ByteArrayOutputStream buffer = new ByteArrayOutputStream();

        int nRead;
        byte[] cache = new byte[16384];

        while ((nRead = inputStream.read(cache, 0, cache.length)) != -1) {
            buffer.write(cache, 0, nRead);
        }

        return buffer.toByteArray();
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (resultCode == Activity.RESULT_CANCELED) {
            Toast.makeText(this, "Canceled", Toast.LENGTH_SHORT).show();
            return;
        }

        if (resultCode == Activity.RESULT_OK && requestCode == SelectImage && data != null) {
            try {
                Uri uriPath = data.getData();

                InputStream is = getContentResolver().openInputStream(uriPath);
                m_ImageData = getBytes(is);
                is.close();

                UnityPlayer.UnitySendMessage("GalleryImage", "ImageDataLoaded", null);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    // Called from C#
    public byte[] getImageData() {
        return m_ImageData;
    }

    // Called from C#
    public void clearImageData() {
        m_ImageData = null;
    }
}
