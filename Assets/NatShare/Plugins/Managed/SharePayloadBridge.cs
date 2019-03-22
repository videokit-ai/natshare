/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class NatShareBridge {

        #if UNITY_IOS && !UNITY_EDITOR
        [DllImport(@"__Internal", EntryPoint = "NSShareText")]
        public static extern bool ShareText (string text);
        [DllImport(@"__Internal", EntryPoint = "NSShareImage")]
        public static extern bool ShareImage (byte[] pngData, int dataSize);
        [DllImport(@"__Internal", EntryPoint = "NSShareMedia")]
        public static extern bool ShareMedia (string media);
        [DllImport(@"__Internal", EntryPoint = "NSSaveImageToCameraRoll")]
        public static extern bool SaveToCameraRoll (byte[] pngData, int dataSize, string album);
        [DllImport(@"__Internal", EntryPoint = "NSSaveMediaToCameraRoll")]
        public static extern bool SaveToCameraRoll (string path, string album, bool copy);
        [DllImport(@"__Internal", EntryPoint = "NSGetThumbnail")]
        public static extern bool GetThumbnail (string path, float time, ref IntPtr pixelBuffer, ref int width, ref int height);

        #else
        public static bool ShareText (string media) { return false; }
        public static bool ShareImage (byte[] pngData, int dataSize) { return false; }
        public static bool ShareMedia (string media) { return false; }
        public static bool SaveToCameraRoll (byte[] pngData, int dataSize, string album) { return false; }
        public static bool SaveToCameraRoll (string path, string album, bool copy) { return false; }
        public static bool GetThumbnail (string path, float time, ref IntPtr pixelBuffer, ref int width, ref int height) { return false; }
        #endif
    }
}