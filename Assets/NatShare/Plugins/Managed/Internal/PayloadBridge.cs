/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba.
*/

namespace NatShare.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class PayloadBridge {

        private const string Assembly = @"__Internal";

        #if UNITY_IOS && !UNITY_EDITOR
        [DllImport(Assembly, EntryPoint = @"NSCreateSharePayload")]
        public static extern IntPtr CreateSharePayload (string subject, Action<IntPtr> completionHandler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NSCreateSavePayload")]
        public static extern IntPtr CreateSavePayload (string album, Action<IntPtr> completionHandler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NSCreatePrintPayload")]
        public static extern IntPtr CreatePrintPayload (bool greyscale, bool landscape, Action<IntPtr> completionHandler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NSAddText")]
        public static extern void AddText (this IntPtr payload, string text);
        [DllImport(Assembly, EntryPoint = @"NSAddImage")]
        public static extern void AddImage (this IntPtr payload, IntPtr pixelBuffer, int width, int height);
        [DllImport(Assembly, EntryPoint = @"NSAddMedia")]
        public static extern void AddMedia (this IntPtr payload, string uri);
        [DllImport(Assembly, EntryPoint = @"NSCommit")]
        public static extern void Commit (this IntPtr payload);

        #else
        public static IntPtr CreateSharePayload (string subject, Action<IntPtr> completionHandler, IntPtr context) => IntPtr.Zero;
        public static IntPtr CreateSavePayload (string album, Action<IntPtr> completionHandler, IntPtr context) => IntPtr.Zero;
        public static IntPtr CreatePrintPayload (bool greyscale, bool landscape, Action<IntPtr> completionHandler, IntPtr context) => IntPtr.Zero;
        public static void AddText (this IntPtr payload, string text) { }
        public static void AddImage (this IntPtr payload, IntPtr pixelBuffer, int width, int height) { }
        public static void AddMedia (this IntPtr payload, string uri) { }
        public static void Commit (this IntPtr payload) { }
        #endif
    }
}