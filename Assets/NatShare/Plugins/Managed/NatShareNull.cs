/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

    using UnityEngine;

    public class NatShareNull : INatShare {

        bool INatShare.Share (byte[] pngData, ShareCallback callback) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
            if (callback != null) callback();
            return true;
        }

        bool INatShare.Share (string media, ShareCallback callback) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
            if (callback != null) callback();
            return true;
        }

        bool INatShare.SaveToCameraRoll (byte[] pngData, string album) {
            Debug.LogError("NatShare Error: This platform does not support saving to camera roll");
            return true;
        }

        bool INatShare.SaveToCameraRoll (string path, string album, bool copy) {
            Debug.LogError("NatShare Error: This platform does not support saving to camera roll");
            return true;
        }

        Texture2D INatShare.GetThumbnail (string videoPath, float time) {
            Debug.LogError("NatShare Error: This platform does not support retrieving thumbnails");
            return null;
        }
    }
}