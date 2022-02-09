/* 
*   NatShare
*   Copyright (c) 2022 Yusuf Olokoba.
*/

namespace NatSuite.Sharing.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class NatShare { // NatShare.h

        private const string Assembly =
        #if UNITY_IOS && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatShare";
        #endif

        public delegate void CompletionHandler (IntPtr context, bool success);

        #region --ISharePayload--
        [DllImport(Assembly, EntryPoint = @"NSSharePayloadAddText")]
        public static extern void AddText (
            this IntPtr payload,
            [MarshalAs(UnmanagedType.LPStr)] string text
        );
        [DllImport(Assembly, EntryPoint = @"NSSharePayloadAddImage")]
        public static extern void AddImage (this IntPtr payload, byte[] jpegData, int dataSize);
        [DllImport(Assembly, EntryPoint = @"NSSharePayloadAddMedia")]
        public static extern void AddMedia (
            this IntPtr payload,
            [MarshalAs(UnmanagedType.LPStr)] string uri
        );
        [DllImport(Assembly, EntryPoint = @"NSSharePayloadCommit")]
        public static extern void Commit (this IntPtr payload, CompletionHandler completionHandler, IntPtr context);
        #endregion


        #region --Constructors--
        [DllImport(Assembly, EntryPoint = @"NSCreateSharePayload")]
        public static extern void CreateSharePayload (out IntPtr payload);
        [DllImport(Assembly, EntryPoint = @"NSCreateSavePayload")]
        public static extern void CreateSavePayload (
            [MarshalAs(UnmanagedType.LPStr)] string album,
            out IntPtr payload
        );
        [DllImport(Assembly, EntryPoint = @"NSCreatePrintPayload")]
        public static extern void CreatePrintPayload (bool color, bool landscape, out IntPtr payload);
        #endregion
    }
}