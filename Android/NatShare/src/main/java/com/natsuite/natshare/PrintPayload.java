package com.natsuite.natshare;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Handler;
import android.os.Looper;
import androidx.print.PrintHelper;
import com.unity3d.player.UnityPlayer;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/09/19.
 */
public final class PrintPayload implements Payload {

    private final Handler delegateHandler; // So we can call back into Unity on main thread
    private final Runnable completionHandler;
    private final PrintHelper printHelper;
    private Bitmap latestImage;

    public PrintPayload (boolean greyscale, boolean landscape) {
        this(greyscale, landscape, null);
    }

    public PrintPayload (boolean greyscale, boolean landscape, Runnable completionHandler) {
        this.delegateHandler = new Handler(Looper.myLooper());
        this.completionHandler = completionHandler;
        this.printHelper = new PrintHelper(UnityPlayer.currentActivity);
        printHelper.setColorMode(greyscale ? PrintHelper.COLOR_MODE_MONOCHROME : PrintHelper.COLOR_MODE_COLOR);
        printHelper.setOrientation(landscape ? PrintHelper.ORIENTATION_LANDSCAPE : PrintHelper.ORIENTATION_PORTRAIT);
    }

    @Override
    public void addText (String text) { } // Not implemented

    @Override
    public void addImage (byte[] pngData) {
        final Bitmap image = BitmapFactory.decodeByteArray(pngData, 0, pngData.length);
        if (latestImage != null)
            latestImage.recycle();
        latestImage = image;
    }

    @Override
    public void addMedia (String uri) { } // Not implemented

    @Override
    public void commit () {
        printHelper.printBitmap("Print", latestImage, new PrintHelper.OnPrintFinishCallback() {
            @Override
            public void onFinish () {
                if (completionHandler != null)
                    delegateHandler.post(completionHandler);
            }
        });
    }
}
