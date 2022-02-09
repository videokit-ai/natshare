/* 
*   NatShare
*   Copyright (c) 2022 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using System;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// Payload for saving to the camera roll.
    /// </summary>
    public sealed class SavePayload : NativePayload {

        #region --Client API--
        /// <summary>
        /// Create a save payload.
        /// </summary>
        /// <param name="album">Optional. Album name in which contents should be saved.</param>
        public SavePayload (string album = default) : base(Create(album)) { }

        /// <summary>
        /// Nop. No concept as saving text to the camera roll.
        /// </summary>
        public override void AddText (string text) => base.AddText(text);
        #endregion


        #region --Operations--

        private static IntPtr Create (string album) {
            switch (Application.platform) {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    NatShare.CreateSavePayload(album, out var payload);
                    return payload;
                default:
                    Debug.LogWarning($"NatShare: SavePayload is not supported on {Application.platform}");
                    return IntPtr.Zero;
            }
        }
        #endregion
    }
}