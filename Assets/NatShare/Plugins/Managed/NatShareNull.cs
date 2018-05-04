/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

    using UnityEngine;
    using System;

    public class NatShareNull : INatShare {

        bool INatShare.Share (byte[] pngData, string message) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
            return false;
        }

        bool INatShare.Share (string path, string message) {
            Debug.LogError("NatShare Error: This platform does not support sharing");
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

        void INatShare.GetThumbnail (string videoPath, Action<Texture2D> callback, float time) {
            Debug.LogError("NatShare Error: This platform does not support retrieving thumbnails");
        }
    }
}