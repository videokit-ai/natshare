/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShare {

    using UnityEngine;
    using System;
    using Internal;

    /// <summary>
    /// A payload for social sharing
    /// </summary>
    [Doc(@"SharePayload")]
    public abstract class SharePayload : IDisposable {

        #region --Client API--
        /// <summary>
        /// Create a share payload.
        /// This function will return `null` on unsupported platforms.
        /// </summary>
        [Doc(@"SharePayloadCreate")]
        public static SharePayload Create (PayloadCommitMode commitMode) {
            switch (Application.platform) {
                case RuntimePlatform.Android: return new SharePayloadAndroid(commitMode);
                case RuntimePlatform.IPhonePlayer: return new SharePayloadiOS(commitMode);
                default: return null;
            }
        }

        /// <summary>
        /// Subject of the share payload
        /// </summary>
        [Doc(@"SetSubject")]
        public abstract SharePayload SetSubject (string subject);

        /// <summary>
        /// Add plain text
        /// </summary>
        [Doc(@"AddText")]
        public abstract SharePayload AddText (string text);

        /// <summary>
        /// Add an image
        /// </summary>
        [Doc(@"AddImage")]
        public abstract SharePayload AddImage (Texture2D image);

        /// <summary>
        /// Add media with at a given URI
        /// </summary>
        [Doc(@"AddMedia")]
        public abstract SharePayload AddMedia (string uri);

        /// <summary>
        /// Commit the payload to the operating system for sharing
        /// </summary>
        [Doc(@"Commit")]
        public abstract void Commit (Action completionHandler = null);

        /// <summary>
        /// Dispose the payload
        /// </summary>
        [Doc(@"Dispose")]
        public abstract void Dispose ();

        /// <summary>
        /// Extract an image from a media file
        /// </summary>
        [Doc(@"ExtractImageFromMedia")]
        public static Texture2D ExtractImageFromMedia (string uri, float time = 0.0f) {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// Share payload commit mode
    /// </summary>
    [Doc(@"PayloadCommitMode")]
    public enum PayloadCommitMode {
        /// <summary>
        /// Share the payload contents
        /// </summary>
        [Doc(@"Share")] Share = 1,
        /// <summary>
        /// Save the payload contents to the camera roll
        /// </summary>
        [Doc(@"SaveToCameraRoll")] SaveToCameraRoll = 2
    }
}