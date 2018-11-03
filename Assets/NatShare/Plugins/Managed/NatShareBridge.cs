/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

    using System;
    using System.Runtime.InteropServices;

    public static class NatShareBridge {

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        "__Internal";
        #else
        "NatShare";
        #endif

        public delegate void ShareCallback (bool completed);

        #if UNITY_IOS && !UNITY_EDITOR
        [DllImport(Assembly, EntryPoint = "NSRegisterCallbacks")]
        public static extern void RegisterCallbacks (ShareCallback callback);
        [DllImport(Assembly, EntryPoint = "NSShareText")]
        public static extern bool ShareText (string text);
        [DllImport(Assembly, EntryPoint = "NSShareImage")]
        public static extern bool ShareImage (byte[] pngData, int dataSize);
        [DllImport(Assembly, EntryPoint = "NSShareMedia")]
        public static extern bool ShareMedia (string media);
        [DllImport(Assembly, EntryPoint = "NSSaveImageToCameraRoll")]
        public static extern bool SaveToCameraRoll (byte[] pngData, int dataSize, string album);
        [DllImport(Assembly, EntryPoint = "NSSaveMediaToCameraRoll")]
        public static extern bool SaveToCameraRoll (string path, string album, bool copy);
        [DllImport(Assembly, EntryPoint = "NSGetThumbnail")]
        public static extern bool GetThumbnail (string path, float time, ref IntPtr pixelBuffer, ref int width, ref int height);
        [DllImport(Assembly, EntryPoint = "NSFreeThumbnail")]
        public static extern void FreeThumbnail (IntPtr pixelBuffer);

        #else
        public static void RegisterCallbacks (ShareCallback callback) {}
        public static bool ShareText (string media) { return false; }
        public static bool ShareImage (byte[] pngData, int dataSize) { return false; }
        public static bool ShareMedia (string media) { return false; }
        public static bool SaveToCameraRoll (byte[] pngData, int dataSize, string album) { return false; }
        public static bool SaveToCameraRoll (string path, string album, bool copy) { return false; }
        public static bool GetThumbnail (string path, float time, ref IntPtr pixelBuffer, ref int width, ref int height) { return false; }
        public static void FreeThumbnail (IntPtr pixelBuffer) {}
        #endif
    }
}