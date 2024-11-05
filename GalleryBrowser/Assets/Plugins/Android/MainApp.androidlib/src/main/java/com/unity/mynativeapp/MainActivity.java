package com.unity.mynativeapp;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.Color;
import android.net.Uri;
import android.os.Bundle;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import android.provider.DocumentsContract;
import android.provider.MediaStore;
import android.provider.OpenableColumns;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayerForActivityOrService;

import java.io.File;
import java.io.IOException;
import java.nio.file.Paths;

public class MainActivity extends UnityPlayerActivity {

    final int SelectImage = 1234;

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

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (resultCode == Activity.RESULT_CANCELED) {
            Toast.makeText(this, "Canceled", Toast.LENGTH_SHORT).show();
            return;
        }

        if (resultCode == Activity.RESULT_OK && requestCode == SelectImage && data != null) {
            try {
                Uri uriPath = data.getData();
                Cursor cursor = getContentResolver().query(uriPath,null, null, null, null);
                cursor.moveToFirst();
                int nameIndex =  cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME);
                String path = new File(getFilesDir(), cursor.getString(nameIndex)).toString();
                cursor.close();

                Toast.makeText(this, path, Toast.LENGTH_SHORT).show();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }
}
