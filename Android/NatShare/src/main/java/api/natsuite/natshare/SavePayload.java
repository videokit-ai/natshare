package api.natsuite.natshare;

import android.content.Intent;
import android.net.Uri;
import android.os.Environment;
import android.os.Handler;
import android.os.HandlerThread;
import android.util.Log;
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
    private final int callback;
    private final ArrayList<byte[]> images = new ArrayList<>();
    private final ArrayList<String> media = new ArrayList<>();

    public SavePayload (String album, int callback) {
        this.saveRoot = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM).getAbsolutePath() + "/" + album);
        this.saveRoot.mkdirs();
        this.callback = callback;
    }

    @Override
    public void addText (String text) { }

    @Override
    public void addImage (final byte[] pngData) {
        images.add(pngData);
    }

    @Override
    public void addMedia (String uri) {
        media.add(uri);
    }

    @Override
    public void commit () {
        final HandlerThread commitThread = new HandlerThread("SavePayload Commit Thread");
        commitThread.start();
        final Handler commitHandler = new Handler(commitThread.getLooper());
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
                        Bridge.activity().sendBroadcast(scanIntent);
                    } catch (IOException ex) {
                        Log.e("Unity", "NatShare Error: SavePayload failed to commit image with error: " + ex);
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
                        Bridge.activity().sendBroadcast(scanIntent);
                    } catch (IOException ex) {
                        Log.e("Unity", "NatShare Error: SavePayload failed to commit media with error: " + ex);
                    }
                // Invoke callback
                if (callback != 0)
                    Bridge.callback(callback);
            }
        });
        commitThread.quitSafely();
    }
}
