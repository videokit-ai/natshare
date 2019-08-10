package com.yusufolokoba.natshare;

import android.graphics.Bitmap;
import android.os.Handler;
import android.os.Looper;
import android.support.v4.print.PrintHelper;
import com.unity3d.player.UnityPlayer;
import java.nio.ByteBuffer;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/09/19.
 */
public final class PrintPayload implements Payload {

    private final Handler delegateHandler; // So we can call back into Unity on main thread
    private final Runnable completionHandler;
    private final PrintHelper printHelper;
    private Bitmap latestImage;

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
    public void addImage (byte[] pixelBuffer, int width, int height) { // INCOMPLETE
        // Load into bitmap
        Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        image.copyPixelsFromBuffer(ByteBuffer.wrap(pixelBuffer));
        // Replace image
        if (latestImage != null)
            latestImage.recycle();
        latestImage = image;
    }

    @Override
    public void addMedia (String uri) { } // Not implemented

    @Override
    public void commit () { // DEPLOY
        printHelper.printBitmap("Print", latestImage, new PrintHelper.OnPrintFinishCallback() {
            @Override
            public void onFinish () {
                if (completionHandler != null)
                    delegateHandler.post(completionHandler);
            }
        });
    }
}
