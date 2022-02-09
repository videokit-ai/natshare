/* 
*   NatShare
*   Copyright (c) 2022 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using System;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// EXPERIMENTAL. Payload for printing media.
    /// </summary>
    public sealed class PrintPayload : NativePayload {

        #region --Client API--
        /// <summary>
        /// Create a print payload.
        /// </summary>
        /// <param name="greyscale">Should items be printed in color. Defaults to `true`.</param>
        /// <param name="landscape">Should items be printed in landscape orientation. Defaults to `false`.</param>
        public PrintPayload (bool color = true, bool landscape = false) : base(Create(color, landscape)) { }
        #endregion


        #region --Operations--

        private static IntPtr Create (bool color, bool landscape) {
            switch (Application.platform) {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    NatShare.CreatePrintPayload(color, landscape, out var payload);
                    return payload;
                default:
                    Debug.LogWarning($"NatShare: PrintPayload is not supported on {Application.platform}");
                    return IntPtr.Zero;
            }
        }
        #endregion
    }
}