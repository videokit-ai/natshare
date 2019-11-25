package com.natsuite.natshare;

import android.content.Intent;
import android.net.Uri;
import android.os.Environment;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Looper;
import com.unity3d.player.UnityPlayer;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SavePayload implements Payload {

    private final File saveRoot;
    private final Runnable callback;
    private final HandlerThread commitThread;
    private final Handler commitHandler, callbackHandler;
    private final ArrayList<byte[]> images = new ArrayList<>();
    private final ArrayList<Uri> media = new ArrayList<>();

    public SavePayload (String album) {
        this(album, null);
    }

    public SavePayload (String album, Runnable callback) {
        this.saveRoot = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM).getAbsolutePath() + "/" + album);
        this.saveRoot.mkdirs();
        this.callback = callback;
        this.callbackHandler = new Handler(Looper.myLooper());
        this.commitThread = new HandlerThread("SavePayload Commit Thread");
        this.commitThread.start();
        this.commitHandler = new Handler(commitThread.getLooper());
    }

    @Override
    public void addText (String text) { }

    @Override
    public void addImage (final byte[] pngData) {
        images.add(pngData);
    }

    @Override
    public void addMedia (String uri) {
        media.add(Uri.fromFile(new File(uri)));
    }

    @Override
    public void commit () { // DEPLOY
        commitHandler.post(new Runnable() {
            @Override
            public void run () {
                // Commit images
                for (byte[] pngData : images)
                    try {
                        File file = new File(saveRoot, System.nanoTime() + ".png");
                        FileOutputStream stream = new FileOutputStream(file);
                        stream.write(pngData);
                        stream.close();
                        Intent scanIntent = new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE, Uri.fromFile(file));
                        UnityPlayer.currentActivity.sendBroadcast(scanIntent);
                    } catch (IOException ex) { }
                // Commit media
                for (Uri uri : media)
                    try {
                        File ifile = new File(uri.toString());
                        File ofile = new File(saveRoot, ifile.getName());
                        FileInputStream istream = new FileInputStream(ifile);
                        FileOutputStream ostream = new FileOutputStream(ofile);
                        istream.getChannel().transferTo(0, istream.getChannel().size(), ostream.getChannel());
                        istream.close();
                        ostream.close();
                        Intent scanIntent = new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE, Uri.fromFile(ofile));
                        UnityPlayer.currentActivity.sendBroadcast(scanIntent);
                    } catch (IOException ex) {}
                // Invoke callback
                callbackHandler.post(callback);
            }
        });
        commitThread.quitSafely();
    }
}
