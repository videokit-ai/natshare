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
import java.util.UUID;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SharePayload implements Payload {

    private final Intent intent;
    private final HandlerThread commitThread;
    private final Handler handler;
    private final ArrayList<Uri> uris;
    private final ArrayList<String> mimes;
    private static final String authority;

    public SharePayload () {
        // Create intent
        this.intent = new Intent();
        this.uris = new ArrayList<>();
        this.mimes = new ArrayList<>();
        // Create handler
        this.commitThread = new HandlerThread("SharePayload");
        commitThread.start();
        this.handler = new Handler(commitThread.getLooper());
        // Set intent params
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
    }

    @Override
    public synchronized void addText (final String text) {
        intent.putExtra(Intent.EXTRA_TEXT, text);
        mimes.add("text/plain");
    }

    @Override
    public synchronized void addImage (final ByteBuffer jpegData) { // CHECK
        // Read into managed memory
        final byte[] buffer = new byte[jpegData.limit()];
        jpegData.get(buffer);
        // Write to file
        handler.post(() -> {
            try {
                File file = new File(UnityPlayer.currentActivity.getCacheDir(), UUID.randomUUID().toString() + ".jpg");
                FileOutputStream stream = new FileOutputStream(file);
                stream.write(buffer);
                stream.close();
                addMedia(file.getAbsolutePath());
            } catch (IOException ex) {
                Log.e("NatSuite", "NatShare Error: SharePayload failed to commit image with error: " + ex);
            }
        });
    }

    @Override
    public synchronized void addMedia (final String path) {
        Uri uri = FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, new File(path));
        String extension = MimeTypeMap.getFileExtensionFromUrl(path);
        String mime = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension);
        uris.add(uri);
        mimes.add(mime);
    }

    @Override
    public synchronized void commit (final Callback completionHandler) {
        handler.post(() -> {
            // Finalize intent
            intent.setAction(uris.size() > 1 ? Intent.ACTION_SEND_MULTIPLE : Intent.ACTION_SEND);
            intent.setType(flattenMime(mimes));
            if (uris.size() == 1)
                intent.putExtra(Intent.EXTRA_STREAM, uris.get(0));
            else if (uris.size() > 1)
                intent.putParcelableArrayListExtra(Intent.EXTRA_STREAM, uris);
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

    private static String flattenMime (ArrayList<String> mimes) {
        // Check for single
        if (mimes.size() == 0)
            return "*/*";
        else if (mimes.size() == 1)
            return mimes.get(0);
        // Find lowest common mime type
        ArrayList<String> mediaTypes = new ArrayList<>();
        for (String mime : mimes) {
            String type = mime.split("/")[0];
            if (!mediaTypes.contains(type))
                mediaTypes.add(type);
        }
        return mediaTypes.size() > 1 ? "*/*" : mediaTypes.get(0) + "/*";
    }

    static { authority = UnityPlayer.currentActivity.getPackageName() + ".natshare"; }
}
