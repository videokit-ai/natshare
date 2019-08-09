package com.yusufolokoba.natshare;

import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/08/19.
 */
public final class SharePayload implements Payload {

    private final Intent intent;

    public SharePayload (String subject, Runnable completionHandler) { // INCOMPLETE
        intent = new Intent().setAction(Intent.ACTION_SEND);
        intent.putExtra(Intent.EXTRA_SUBJECT, subject);
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
    }

    @Override
    public void dispose () { // INCOMPLETE

    }

    @Override
    public void addText (String text) { // DEPLOY
        intent.putExtra(Intent.EXTRA_TEXT, text);
    }

    @Override
    public void addImage (byte[] pixelBuffer, int width, int height) { // INCOMPLETE
        // Load into bitmap
        Bitmap image = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        image.copyPixelsFromBuffer(ByteBuffer.wrap(pixelBuffer));
        // ...
    }

    @Override
    public void addMedia (String uri) { // DEPLOY
        intent.putExtra(Intent.EXTRA_STREAM, Uri.parse(uri));
    }

    @Override
    public void commit () { // INCOMPLETE

    }
}
