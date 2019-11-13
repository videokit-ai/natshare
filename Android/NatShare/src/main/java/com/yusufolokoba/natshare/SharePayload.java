package com.yusufolokoba.natshare;

import android.app.Fragment;
import android.app.PendingIntent;
import android.content.Intent;
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

    public SharePayload (String subject) {
        this(subject, null);
    }

    public SharePayload (String subject, Runnable completionHandler) { // INCOMPLETE // completion handler
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
    public void addImage (final byte[] pngData) {
        // Write to file
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                // Write images
                try {
                    File file = new File(UnityPlayer.currentActivity.getCacheDir(), "share." + System.nanoTime() + ".png");
                    FileOutputStream outputStream = new FileOutputStream(file);
                    outputStream.write(pngData);
                    outputStream.close();
                    Uri fileUri = FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, file);
                    uris.add(fileUri);
                } catch (IOException ex) {
                    Log.e("Unity", "NatShare Error: SharePayload failed to commit image with error: " + ex);
                }
            }
        });
    }

    @Override
    public void addMedia (final String uri) {
        final Uri contentUri = FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, new File(uri));
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                uris.add(contentUri);
            }
        });
    }

    @Override
    public void commit () { // INCOMPLETE // Put extra, delegate handle?
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
                Intent receiver = new Intent(UnityPlayer.currentActivity, ShareReceiver.class);
                PendingIntent pendingIntent = PendingIntent.getBroadcast(UnityPlayer.currentActivity, 0, receiver, PendingIntent.FLAG_UPDATE_CURRENT);
                Intent chooser = Intent.createChooser(intent, null, pendingIntent.getIntentSender());
                UnityPlayer.currentActivity.startActivity(chooser);
            }
        });
        commitThread.quitSafely();
    }

    static { authority = UnityPlayer.currentActivity.getPackageName() + ".natshare"; }
}
