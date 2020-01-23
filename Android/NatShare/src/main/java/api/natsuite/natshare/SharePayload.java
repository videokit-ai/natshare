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

    private final Intent intent;
    private final CompletionHandler completionHandler;
    private final ArrayList<byte[]> images;
    private final ArrayList<Uri> uris;
    private static final String authority;

    static { authority = UnityPlayer.currentActivity.getPackageName() + ".natshare"; }

    public SharePayload (CompletionHandler completionHandler) {
        // Create intent
        this.intent = new Intent();
        this.completionHandler = completionHandler;
        this.images = new ArrayList<>();
        this.uris = new ArrayList<>();
        // Set intent params
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
    }

    @Override
    public void addText (String text) {
        intent.putExtra(Intent.EXTRA_TEXT, text);
    }

    @Override
    public void addImage (final byte[] pngData) {
        images.add(pngData);
    }

    @Override
    public void addMedia (final String uri) {
        uris.add(FileProvider.getUriForFile(UnityPlayer.currentActivity, authority, new File(uri)));
    }

    @Override
    public void commit () {
        final HandlerThread commitThread = new HandlerThread("SharePayload Commit Thread");
        commitThread.start();
        new Handler(commitThread.getLooper()).post(new Runnable() {
            @Override
            public void run () {
                // Write images
                for (byte[] pngData : images)
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
                // Finalize intent
                intent.setType("*/*");
                if (uris.size() > 1) {
                    intent.setAction(Intent.ACTION_SEND_MULTIPLE);
                    intent.putParcelableArrayListExtra(Intent.EXTRA_STREAM, uris);
                }
                else if (uris.size() == 1) {
                    intent.setAction(Intent.ACTION_SEND);
                    intent.putExtra(Intent.EXTRA_STREAM, uris.get(0));
                }
                // Start activity
                ShareReceiver.completionHandler = completionHandler;
                Intent receiver = new Intent(UnityPlayer.currentActivity, ShareReceiver.class);
                PendingIntent pendingIntent = PendingIntent.getBroadcast(UnityPlayer.currentActivity, 0, receiver, PendingIntent.FLAG_UPDATE_CURRENT);
                Intent chooser = Intent.createChooser(intent, null, pendingIntent.getIntentSender());
                UnityPlayer.currentActivity.startActivity(chooser);
            }
        });
        commitThread.quitSafely();
    }

    public static final class ShareReceiver extends BroadcastReceiver {

        public static CompletionHandler completionHandler;

        @Override
        public void onReceive (final Context context, final Intent intent) {
            final ComponentName clickedComponent = intent.getParcelableExtra(Intent.EXTRA_CHOSEN_COMPONENT);
            completionHandler.onCompletion(true);
        }
    }
}
