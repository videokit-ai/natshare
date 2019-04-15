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
    public class SharePayload : IDisposable {

        #region --Client API--
        /// <summary>
        /// Create a share payload
        /// </summary>
        [Doc(@"SharePayloadCtor")]
        public SharePayload () {
            switch (Application.platform) {
                case RuntimePlatform.Android: backingPayload = new SharePayloadAndroid(); break;
                case RuntimePlatform.IPhonePlayer: backingPayload = new SharePayloadiOS(); break;
                default: backingPayload = null; break;
            }
        }

        /// <summary>
        /// Subject of the share payload
        /// </summary>
        [Doc(@"SetSubject")]
        public virtual SharePayload SetSubject (string subject) {
            return backingPayload.SetSubject(subject);
        }

        /// <summary>
        /// Add plain text
        /// </summary>
        [Doc(@"AddText")]
        public virtual SharePayload AddText (string text) {
            return backingPayload.AddText(text);
        }

        /// <summary>
        /// Add an image
        /// </summary>
        [Doc(@"AddImage")]
        public virtual SharePayload AddImage (Texture2D image) {
            return backingPayload.AddImage(image);
        }

        /// <summary>
        /// Add media with at a given URI
        /// </summary>
        [Doc(@"AddMedia")]
        public virtual SharePayload AddMedia (string uri) {
            return backingPayload.AddMedia(uri);
        }

        /// <summary>
        /// Share the payload
        /// </summary>
        /// <param name="completionHandler">Delegate invoked with whether sharing was successful</param>
        [Doc(@"Share")]
        public virtual void Share (Action<bool> completionHandler = null) {
            backingPayload.Share(completionHandler);
        }

        /// <summary>
        /// Save the payload to the device camera roll
        /// </summary>
        /// <param name="completionHandler">Delegate invoked with whether sharing was successful</param>
        [Doc(@"Save")]
        public virtual void Save (string album = null, Action<bool> completionHandler = null) {
            backingPayload.Save(album, completionHandler);
        }

        /// <summary>
        /// Dispose the payload
        /// </summary>
        [Doc(@"Dispose")]
        public virtual void Dispose () {
            backingPayload.Dispose();
        }

        /// <summary>
        /// Extract an image from a media file
        /// </summary>
        [Doc(@"ExtractImageFromMedia")]
        public static Texture2D ExtractImageFromMedia (string uri, float time = 0.0f) {
            return null;
        }
        #endregion

        private readonly SharePayload backingPayload;
    }
}