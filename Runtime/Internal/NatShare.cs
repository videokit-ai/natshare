/* 
*   NatShare
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Sharing.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class NatShare { // NatShare.h

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatShare";
        #endif


        #region --Delegates--
        public delegate void ShareResultHandler (IntPtr context, IntPtr receiver);
        public delegate void SaveResultHandler (IntPtr context, bool success);
        public delegate void SavePermissionHandler (IntPtr context, bool granted);
        #endregion


        #region --SharePayload--
        [DllImport(Assembly, EntryPoint = @"NMLCreateSharePayload")]
        public static extern void CreateSharePayload (out IntPtr payload);

        [DllImport(Assembly, EntryPoint = @"NMLReleaseSharePayload")]
        public static extern void ReleaseSharePayload (this IntPtr payload);

        [DllImport(Assembly, EntryPoint = @"NMLSharePayloadAddText")]
        public static extern void AddSharePayloadText (
            this IntPtr payload,
            [MarshalAs(UnmanagedType.LPStr)] string text
        );

        [DllImport(Assembly, EntryPoint = @"NMLSharePayloadAddImage")]
        public static extern void AddSharePayloadImage (
            this IntPtr payload,
            byte[] imageBuffer,
            int bufferSize
        );

        [DllImport(Assembly, EntryPoint = @"NMLSharePayloadAddMedia")]
        public static extern void AddSharePayloadMedia (
            this IntPtr payload,
            [MarshalAs(UnmanagedType.LPStr)] string uri
        );

        [DllImport(Assembly, EntryPoint = @"NMLSharePayloadShare")]
        public static extern void Share (
            this IntPtr payload,
            ShareResultHandler handler,
            IntPtr context
        );
        #endregion


        #region --SavePayload--
        [DllImport(Assembly, EntryPoint = @"NMLCreateSavePayload")]
        public static extern void CreateSavePayload (
            [MarshalAs(UnmanagedType.LPStr)] string album,
            out IntPtr payload
        );

        [DllImport(Assembly, EntryPoint = @"NMLReleaseSavePayload")]
        public static extern void ReleaseSavePayload (this IntPtr payload);

        [DllImport(Assembly, EntryPoint = @"NMLSavePayloadAddImage")]
        public static extern void AddSavePayloadImage (
            this IntPtr payload,
            byte[] imageBuffer,
            int bufferSize
        );

        [DllImport(Assembly, EntryPoint = @"NMLSavePayloadAddMedia")]
        public static extern void AddSavePayloadMedia (
            this IntPtr payload,
            [MarshalAs(UnmanagedType.LPStr)] string uri
        );

        [DllImport(Assembly, EntryPoint = @"NMLSavePayloadSave")]
        public static extern void Save (
            this IntPtr payload,
            SaveResultHandler handler,
            IntPtr context
        );

        [DllImport(Assembly, EntryPoint = @"NMLSavePayloadRequestPermissions")]
        public static extern void RequestPermissions (
            SavePermissionHandler handler,
            IntPtr context
        );
        #endregion


        #region --MediaPicker--
        [DllImport(Assembly, EntryPoint = @"NMLCreateMediaPicker")]
        public static extern void CreateMediaPicker (out IntPtr picker);

        [DllImport(Assembly, EntryPoint = @"NMLReleaseMediaPicker")]
        public static extern void ReleaseMediaPicker (this IntPtr picker);

        [DllImport(Assembly, EntryPoint = @"NMLMediaPickerSetMIMETypes")]
        public static extern void SetMIMETypes (
            this IntPtr picker,
            [In, MarshalAs(UnmanagedType.LPStr)] string[] mimes,
            int count
        );

        [DllImport(Assembly, EntryPoint = @"NMLMediaPickerSetMaxPicks")]
        public static extern void SetMaxPicks (
            this IntPtr picker,
            int count
        );
        #endregion
    }
}