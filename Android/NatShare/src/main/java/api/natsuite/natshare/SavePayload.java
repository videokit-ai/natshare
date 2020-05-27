package api.natsuite.natshare;

import android.content.Intent;
import android.net.Uri;
import android.os.Environment;
import android.os.Handler;
import android.os.HandlerThread;
import android.util.Log;
import com.unity3d.player.UnityPlayer;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SavePayload implements Payload {

    private final File saveRoot;
    private final Callback completionHandler;
    private final ArrayList<byte[]> images;
    private final ArrayList<String> media;

    public SavePayload (String album, Callback completionHandler) {
        this.saveRoot = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM).getAbsolutePath() + "/" + album);
        this.completionHandler = completionHandler;
        this.images = new ArrayList<>();
        this.media = new ArrayList<>();
        saveRoot.mkdirs();
    }

    @Override
    public void addText (String text) { }

    @Override
    public void addImage (final ByteBuffer jpegData) {
        images.add(pngData);
    }

    @Override
    public void addMedia (final String path) {
        media.add(path);
    }

    @Override
    public void commit () {
        final HandlerThread commitThread = new HandlerThread("SavePayload Commit Thread");
        commitThread.start();
        new Handler(commitThread.getLooper()).post(new Runnable() {
            @Override
            public void run () {
                boolean success = true;
                // Commit images
                for (byte[] pngData : images)
                    try {
                        File file = new File(saveRoot, System.nanoTime() + ".png");
                        FileOutputStream stream = new FileOutputStream(file);
                        stream.write(pngData);
                        stream.close();
                        Intent scanIntent = new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE, Uri.fromFile(file));
                        UnityPlayer.currentActivity.sendBroadcast(scanIntent);
                    } catch (IOException ex) {
                        Log.e("NatSuite", "NatShare Error: SavePayload failed to commit image with error: " + ex);
                        success = false;
                    }
                // Commit media
                for (String uri : media)
                    try {
                        File ifile = new File(uri);
                        File ofile = new File(saveRoot, ifile.getName());
                        FileInputStream istream = new FileInputStream(ifile);
                        FileOutputStream ostream = new FileOutputStream(ofile);
                        istream.getChannel().transferTo(0, istream.getChannel().size(), ostream.getChannel());
                        istream.close();
                        ostream.close();
                        Intent scanIntent = new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE, Uri.fromFile(ofile));
                        UnityPlayer.currentActivity.sendBroadcast(scanIntent);
                    } catch (IOException ex) {
                        Log.e("NatSuite", "NatShare Error: SavePayload failed to commit media with error: " + ex);
                        success = false;
                    }
                // Invoke callback
                completionHandler.onCompletion(success);
            }
        });
        commitThread.quitSafely();
    }
}
