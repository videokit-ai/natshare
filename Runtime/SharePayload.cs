/* 
*   NatShare
*   Copyright (c) 2022 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using System;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// Payload for sharing to available social services.
    /// </summary>
    public sealed class SharePayload : NativePayload {

        #region --Client API--
        /// <summary>
        /// Create a share payload.
        /// </summary>
        public SharePayload () : base(Create()) { }
        #endregion


        #region --Operations--

        private static IntPtr Create () {
            switch (Application.platform) {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    NatShare.CreateSharePayload(out var payload);
                    return payload;
                default:
                    Debug.LogWarning($"NatShare: SharePayload is not supported on {Application.platform}");
                    return IntPtr.Zero;
            }
        }
        #endregion
    }
}