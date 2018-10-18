package com.yusufolokoba.natshare;

import android.app.Activity;
import android.app.Fragment;
import android.content.Intent;

/**
 * NatShare
 * Created by Sean Roske on 10/07/18.
 */
public class NatShareCallbacks extends Fragment {

    private NatShareDelegate delegate;
    public static final String TAG = "NatShareCallbacks";
    public static final int ACTIVITY_SHARE_TEXT = 41;
    public static final int ACTIVITY_SHARE_IMAGE = 42;
    public static final int ACTIVITY_SHARE_MEDIA = 43;

    public void setDelegate (NatShareDelegate delegate) {
        this.delegate = delegate;
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        delegate.onShare(resultCode == Activity.RESULT_OK);
    }
}
