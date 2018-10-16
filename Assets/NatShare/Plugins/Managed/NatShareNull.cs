/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

    using UnityEngine;

    public class NatShareNull : INatShare {

        bool INatShare.ShareText (string text, ShareCallback callback) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
            if (callback != null) callback();
            return false;
		}

        bool INatShare.ShareImage (byte[] pngData, string message, ShareCallback callback) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
            if (callback != null) callback();
            return false;
        }

        bool INatShare.ShareMedia (string path, string message, ShareCallback callback) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
            if (callback != null) callback();
            return false;
        }

        bool INatShare.SaveToCameraRoll (byte[] pngData) {
            Debug.LogError("NatShare Error: This platform does not support saving to camera roll");
            return false;
        }

        bool INatShare.SaveToCameraRoll (string videoPath) {
            Debug.LogError("NatShare Error: This platform does not support saving to camera roll");
            return false;
        }

        Texture2D INatShare.GetThumbnail (string videoPath, float time) {
            Debug.LogError("NatShare Error: This platform does not support retrieving thumbnails");
            return null;
        }
    }
}