package com.yusufolokoba.natshare;

import android.content.Intent;
import android.net.Uri;
import java.util.ArrayList;

public final class SharePayload {

    private final Intent intent = new Intent().setAction(Intent.ACTION_SEND);

    public interface Callback {
        void onShare (boolean success);
    }

    public SharePayload () {

    }

    public void setSubject (String subject) {
        intent.putExtra(Intent.EXTRA_SUBJECT, subject);
    }

    public void addText (String text) {
        intent.putExtra(Intent.EXTRA_TEXT, text);
    }

    public void addImage (byte[] pngData) {

    }

    public void addMedia (String uri) {
        intent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION).putExtra(Intent.EXTRA_STREAM, Uri.parse(uri));
    }

    public void share (Callback callback) {

    }

    public void save (String album, Callback callback) {

    }
}
