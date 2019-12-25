package api.natsuite.natshare;

import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Handler;
import android.os.HandlerThread;
import androidx.core.content.FileProvider;
import android.util.Log;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SharePayload implements Payload {

    private final Intent intent;
    private final int callback;
    private final HandlerThread commitThread;
    private final Handler commitHandler;
    private final ArrayList<Uri> uris = new ArrayList<>();

    public SharePayload (String subject, int callback) {
        // Create intent
        this.callback = callback;
        this.intent = new Intent();
        intent.putExtra(Intent.EXTRA_SUBJECT, subject);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        // Create commit handler
        this.commitThread = new HandlerThread("SharePayload Commit Thread");
        this.commitThread.start();
        this.commitHandler = new Handler(commitThread.getLooper());
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
                    File file = new File(Bridge.activity().getCacheDir(), "share." + System.nanoTime() + ".png");
                    FileOutputStream outputStream = new FileOutputStream(file);
                    outputStream.write(pngData);
                    outputStream.close();
                    Uri fileUri = FileProvider.getUriForFile(Bridge.activity(), authority, file);
                    uris.add(fileUri);
                } catch (IOException ex) {
                    Log.e("Unity", "NatShare Error: SharePayload failed to commit image with error: " + ex);
                }
            }
        });
    }

    @Override
    public void addMedia (final String uri) {
        final Uri contentUri = FileProvider.getUriForFile(Bridge.activity(), authority, new File(uri));
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
                Intent receiver = new Intent(Bridge.activity(), ShareReceiver.class);
                receiver.putExtra("context", callback);
                PendingIntent pendingIntent = PendingIntent.getBroadcast(Bridge.activity(), 0, receiver, PendingIntent.FLAG_UPDATE_CURRENT);
                Intent chooser = Intent.createChooser(intent, null, pendingIntent.getIntentSender());
                Bridge.activity().startActivity(chooser);
            }
        });
        commitThread.quitSafely();
    }

    public static final class ShareReceiver extends BroadcastReceiver {

        @Override
        public void onReceive (final Context context, final Intent intent) {
            final ComponentName clickedComponent = intent.getParcelableExtra(Intent.EXTRA_CHOSEN_COMPONENT);
            final int callback = intent.getIntExtra("context", 0);
            if (callback != 0)
                Bridge.callback(callback);
        }
    }

    private static final String authority;
    static { authority = Bridge.activity().getPackageName() + ".natshare"; }
}
