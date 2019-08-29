package com.yusufolokoba.natshare;

import android.app.Fragment;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Looper;
import androidx.core.content.FileProvider;
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

    private final HandlerThread commitThread;
    private final Handler commitHandler;
    private final Intent intent;
    private final ArrayList<Uri> uris;
    private static final String authority;

    public SharePayload (String subject, Runnable completionHandler) { // INCOMPLETE
        this.commitThread = new HandlerThread("SharePayload Commit Thread");
        this.commitThread.start();
        this.commitHandler = new Handler(commitThread.getLooper());
        // Create intent
        this.intent = new Intent();
        if (!subject.equals(""))
            intent.putExtra(Intent.EXTRA_SUBJECT, subject);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        // Create collections
        this.uris = new ArrayList<>();
        // Setup completion handler
        String payloadID = "SharePayload." + System.nanoTime();
        final ResultHandler resultHandler = new ResultHandler();
        resultHandler.setHandler(completionHandler);
        UnityPlayer.currentActivity.getFragmentManager().beginTransaction().add(resultHandler, payloadID).commit();
    }

    @Override
    public void addText (String text) {
        intent.putExtra(Intent.EXTRA_TEXT, text);
    }

    @Override
    public void addImage (final byte[] pixelBuffer, final int width, final int height) { // DEPLOY
        // Load into bitmap
        final Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        image.copyPixelsFromBuffer(ByteBuffer.wrap(pixelBuffer));
        // Write to file
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                // Write images
                try {
                    File file = new File(UnityPlayer.currentActivity.getCacheDir(), "share." + System.nanoTime() + ".png");
                    FileOutputStream outputStream = new FileOutputStream(file);
                    image.compress(Bitmap.CompressFormat.PNG, 100, outputStream);
                    outputStream.close();
                    Uri fileUri = FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, file);
                    Log.d("Unity", "Created URI for image: "+fileUri);
                    uris.add(fileUri);
                } catch (IOException ex) {
                    Log.e("Unity", "NatShare Error: SharePayload failed to commit image with error: " + ex);
                } finally {
                    image.recycle();
                }
            }
        });
    }

    @Override
    public void addMedia (final String uri) { // DEPLOY
        final Uri contentUri = FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, new File(uri));
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                uris.add(contentUri);
            }
        });
    }

    @Override
    public void commit () {
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                // Set intent type
                intent.setAction(uris.size() > 1 ? Intent.ACTION_SEND_MULTIPLE : Intent.ACTION_SEND);
                intent.setType("*/*");
                // Add URI's
                if (uris.size() > 1)
                    intent.putParcelableArrayListExtra(Intent.EXTRA_STREAM, uris);
                else if (uris.size() == 1)
                    intent.putExtra(Intent.EXTRA_STREAM, uris.get(0));
                // Start activity
                UnityPlayer.currentActivity.startActivityForResult(Intent.createChooser(intent, "Share"), 0);
            }
        });
        commitThread.quitSafely();
    }

    static { authority = UnityPlayer.currentActivity.getPackageName() + ".natshare"; }

    /**
     * Created by Sean Roske on 10/07/18.
     */
    public static class ResultHandler extends Fragment {

        private Runnable delegate;
        private Handler handler;

        public void setHandler (Runnable delegate) {
            this.delegate = delegate;
            this.handler = new Handler(Looper.myLooper());
        }

        @Override
        public void onActivityResult (int requestCode, int resultCode, Intent data) {
            handler.post(delegate);
        }
    }
}
