package com.yusufolokoba.natshare;

import android.graphics.Bitmap;
import java.nio.ByteBuffer;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SavePayload implements Payload {

    public SavePayload (String album, Runnable completionHandler) {

    }

    @Override
    public void dispose () { // INCOMPLETE

    }

    @Override
    public void addText (String text) { }

    @Override
    public void addImage (byte[] pixelBuffer, int width, int height) { // INCOMPLETE
        // Load into bitmap
        Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        image.copyPixelsFromBuffer(ByteBuffer.wrap(pixelBuffer));
        // ...
    }

    @Override
    public void addMedia (String uri) { // INCOMPLETE

    }

    @Override
    public void commit () { // INCOMPLETE

    }
}
