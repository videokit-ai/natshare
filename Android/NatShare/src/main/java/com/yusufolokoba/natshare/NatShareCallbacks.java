package com.yusufolokoba.natshare;

import android.app.Fragment;
import android.content.Intent;

import com.unity3d.player.UnityPlayer;

import static android.app.Activity.RESULT_CANCELED;
import static android.app.Activity.RESULT_OK;

public class NatShareCallbacks extends Fragment {

    public static final String TAG = "NatShareCallbacks";

    public static final int ACTIVITY_SHARE_IMAGE = 42;
    public static final int ACTIVITY_SHARE_VIDEO = 43;

    private String gameObjectName;
    public void setGameObjectName(String name) {
        gameObjectName = name;
    }

    private String successMethodName;
    public void setSuccessMethodName(String name) {
        successMethodName = name;
    }

    private String failureMethodName;
    public void setFailureMethodName(String name) {
        failureMethodName = name;
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        String activityName = "activity.unknown";
        switch (requestCode) {
            case ACTIVITY_SHARE_IMAGE:
                activityName = "activity.share.image";
                break;
            case ACTIVITY_SHARE_VIDEO:
                activityName = "activity.share.video";
                break;
        }
        if (resultCode == RESULT_OK) {
            UnityPlayer.UnitySendMessage(gameObjectName, successMethodName, activityName);
        } else if (resultCode == RESULT_CANCELED) {
            UnityPlayer.UnitySendMessage(gameObjectName, failureMethodName, activityName);
        }
    }
}
