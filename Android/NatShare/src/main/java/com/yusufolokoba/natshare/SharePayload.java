package com.yusufolokoba.natshare;

import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Environment;
import android.util.Log;
import com.unity3d.player.UnityPlayer;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SharePayload implements Payload {

    private final Intent intent;
    private final ArrayList<Bitmap> images;

    public SharePayload (String subject, Runnable completionHandler) { // INCOMPLETE
        intent = new Intent().setAction(Intent.ACTION_SEND);
        intent.putExtra(Intent.EXTRA_SUBJECT, subject);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        images = new ArrayList<>();
    }

    @Override
    public void addText (String text) { // DEPLOY
        intent.putExtra(Intent.EXTRA_TEXT, text);
    }

    @Override
    public void addImage (byte[] pixelBuffer, int width, int height) { // INCOMPLETE
        // Load into bitmap
        Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        image.copyPixelsFromBuffer(ByteBuffer.wrap(pixelBuffer));
        images.add(image);
        // Write to file
        try {
            File file = new File(UnityPlayer.currentActivity.getExternalCacheDir(), System.nanoTime() + ".png");
            FileOutputStream outputStream = new FileOutputStream(file);
            image.compress(Bitmap.CompressFormat.PNG, 100, outputStream);
            outputStream.close();
            intent.putExtra(Intent.EXTRA_STREAM, Uri.fromFile(file));
            intent.setType("image/png");
        } catch (IOException ex) {
            Log.e("Unity", "NatShare Error: Failed to add image to share payload with error: " + ex);
        }
    }

    @Override
    public void addMedia (String uri) { // DEPLOY
        intent.putExtra(Intent.EXTRA_STREAM, Uri.parse(uri));
    }

    @Override
    public void commit () { // INCOMPLETE
        // ...
        for (Bitmap image : images)
            image.recycle();
    }
}
