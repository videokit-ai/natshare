package api.natsuite.natshare;

import android.app.Activity;
import android.os.Handler;
import android.os.Looper;
import com.unity3d.player.UnityPlayer;

/**
 * NatShare
 * Created by Yusuf Olokoba on 12/15/19.
 */
public final class Bridge {

    interface CompletionHandler {
        void onCompletion (int callback);
    }

    public static Activity activity () {
        return UnityPlayer.currentActivity;
    }

    public static void callback (final int id) {
        handler.post(new Runnable() {
            @Override
            public void run() {
                callback.onCompletion(id);
            }
        });
    }

    private static void setCallback (CompletionHandler completionHandler) {
        /**
         * We can't use Unity's `AndroidJavaProxy` as-is because the `SharePayload` can't work with it
         * So we revert to a C-style contextual delegate system (like in Objective-C)
         */
        callback = completionHandler;
        handler = new Handler(Looper.myLooper());
    }

    private static CompletionHandler callback;
    private static Handler handler;
}
