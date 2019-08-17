package com.yusufolokoba.natshare;

import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Looper;
import android.support.v4.content.FileProvider;
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

    private final Runnable completionHandler;
    private final Intent intent;
    private final ArrayList<Bitmap> images;
    private final ArrayList<Uri> uris;

    public SharePayload (String subject, Runnable completionHandler) { // INCOMPLETE
        this.completionHandler = completionHandler;
        // Create intent
        intent = new Intent();
        intent.putExtra(Intent.EXTRA_SUBJECT, subject);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        // Create collections
        images = new ArrayList<>();
        uris = new ArrayList<>();
    }

    @Override
    public void addText (String text) {
        intent.putExtra(Intent.EXTRA_TEXT, text);
    }

    @Override
    public void addImage (byte[] pixelBuffer, int width, int height) { // INCOMPLETE
        // Load into bitmap
        Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        image.copyPixelsFromBuffer(ByteBuffer.wrap(pixelBuffer));
        images.add(image);
        // Write to file
        /*
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
        */
    }

    @Override
    public void addMedia (String uri) { // DEPLOY
        Uri contentUri = FileProvider.getUriForFile(UnityPlayer.currentActivity, UnityPlayer.currentActivity.getPackageName() + ".natshare", new File(uri));
        uris.add(contentUri);
        Log.d("Unity", "Content URI: "+contentUri);
    }

    @Override
    public void commit () { // INCOMPLETE
        // Create worker thread for IO
        final HandlerThread commitThread = new HandlerThread("SharePayload Commit Thread");
        commitThread.start();
        final Handler commitHandler = new Handler(commitThread.getLooper());
        final Handler delegateHandler = new Handler(Looper.myLooper());
        // Commit
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                // Write images
                for (int i = 0; i < images.size(); i++) {
                    final Bitmap image = images.get(i);
                    try {
                        File file = new File(UnityPlayer.currentActivity.getCacheDir(), "natshare." + i + ".png");
                        FileOutputStream outputStream = new FileOutputStream(file);
                        image.compress(Bitmap.CompressFormat.PNG, 100, outputStream);
                        outputStream.close();
                        Uri fileUri = FileProvider.getUriForFile(UnityPlayer.currentActivity, UnityPlayer.currentActivity.getPackageName() + ".natshare", file);
                        uris.add(fileUri);
                    } catch (IOException ex) {
                        Log.e("Unity", "NatShare Error: SharePayload failed to commit image with error: " + ex);
                    } finally {
                        image.recycle();
                    }
                }
                images.clear();
                //
            }
        });
        commitThread.quitSafely();
        // Set action

    }
}
