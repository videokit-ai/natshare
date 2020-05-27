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
import android.webkit.MimeTypeMap;

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
    private final Callback completionHandler;
    private final ArrayList<Uri> uris;
    private static final String authority;
    private String mime; // When sharing single item

    static { authority = UnityPlayer.currentActivity.getPackageName() + ".natshare"; }

    public SharePayload (Callback completionHandler) {
        // Create intent
        this.intent = new Intent();
        this.completionHandler = completionHandler;
        this.uris = new ArrayList<>();
        // Set intent params
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
    }

    @Override
    public void addText (final String text) {
        intent.putExtra(Intent.EXTRA_TEXT, text);
        mime = "text/plain";
    }

    @Override
    public void addImage (final ByteBuffer jpegData) {
        // Read into managed memory
        final byte[] buffer = new byte[jpegData.capacity()];
        jpegData.clear();
        jpegData.get(buffer);
        // Write to file
        try {
            File file = new File(UnityPlayer.currentActivity.getCacheDir(), "share." + System.nanoTime() + ".jpg");
            FileOutputStream outputStream = new FileOutputStream(file);
            outputStream.write(buffer);
            outputStream.close();
            addMedia(file.getAbsolutePath());
            mime = "image/jpeg";
        } catch (IOException ex) {
            Log.e("NatSuite", "NatShare Error: SharePayload failed to commit image with error: " + ex);
        }
    }

    @Override
    public void addMedia (final String path) {
        Uri uri = FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, new File(path));
        uris.add(uri);
        String extension = MimeTypeMap.getFileExtensionFromUrl(path);
        mime = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension);
    }

    @Override
    public void commit () {
        final HandlerThread commitThread = new HandlerThread("SharePayload Commit Thread");
        commitThread.start();
        new Handler(commitThread.getLooper()).post(() -> {
            // Finalize intent
            if (uris.size() > 1) {
                intent.setAction(Intent.ACTION_SEND_MULTIPLE);
                intent.putParcelableArrayListExtra(Intent.EXTRA_STREAM, uris);
                intent.setType("*/*");
            }
            else if (uris.size() == 1) {
                intent.setAction(Intent.ACTION_SEND);
                intent.putExtra(Intent.EXTRA_STREAM, uris.get(0));
                intent.setType(mime);
            }
            // Start activity
            ShareReceiver.completionHandler = completionHandler;
            Intent receiver = new Intent(UnityPlayer.currentActivity, ShareReceiver.class);
            PendingIntent pendingIntent = PendingIntent.getBroadcast(UnityPlayer.currentActivity, 0, receiver, PendingIntent.FLAG_UPDATE_CURRENT);
            Intent chooser = Intent.createChooser(intent, null, pendingIntent.getIntentSender());
            UnityPlayer.currentActivity.startActivity(chooser);
        });
        commitThread.quitSafely();
    }

    public static final class ShareReceiver extends BroadcastReceiver {

        public static Callback completionHandler;

        @Override
        public void onReceive (final Context context, final Intent intent) {
            final ComponentName clickedComponent = intent.getParcelableExtra(Intent.EXTRA_CHOSEN_COMPONENT);
            completionHandler.onCompletion(true);
        }
    }
}
