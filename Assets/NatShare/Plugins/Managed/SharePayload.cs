/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba.
*/

namespace NatShare {

    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using Internal;

    /// <summary>
    /// A payload for sharing to available social services
    /// </summary>
    [Doc(@"SharePayload")]
    public sealed class SharePayload : ISharePayload {

        #region --Client API--
        /// <summary>
        /// Create a share payload
        /// </summary>
        /// <param name="subject">Optional. Subject attached to the sharing payload</param>
        /// <param name="completionHandler">Optional. Delegate invoked with whether sharing was successful</param>
        [Doc(@"SharePayloadCtor")]
        public SharePayload (string subject = null, Action completionHandler = null) {
            switch (Application.platform) {
                case RuntimePlatform.Android: {
                    var nativePayload = new AndroidJavaObject(@"api.natsuite.natshare.SharePayload", subject ?? "", PayloadAndroid.GetCallbackID(completionHandler));
                    this.payload = new PayloadAndroid(nativePayload);
                    break;
                }
                case RuntimePlatform.IPhonePlayer: {
                    var callback = completionHandler != null ? (IntPtr)GCHandle.Alloc(completionHandler, GCHandleType.Normal) : IntPtr.Zero;
                    var nativePayload = PayloadBridge.CreateSharePayload(subject, PayloadiOS.OnCompletion, callback);
                    this.payload = new PayloadiOS(nativePayload);
                    break;
                }
                default:
                    Debug.LogError("NatShare Error: SharePayload is not supported on this platform");
                    this.payload = null; // Self-destruct >:D
                    break;
            }
        }

        /// <summary>
        /// Dispose the payload
        /// </summary>
        [Doc(@"Dispose")]
        public void Dispose () => payload.Dispose();

        /// <summary>
        /// Add plain text
        /// </summary>
        [Doc(@"AddText")]
        public void AddText (string text) => payload.AddText(text);

        /// <summary>
        /// Add an image to the payload
        /// </summary>
        /// <param name="image">Image</param>
        [Doc(@"AddImage")]
        public void AddImage (Texture2D image) => payload.AddImage(image);

        /// <summary>
        /// Add media to the payload
        /// </summary>
        /// <param name="path">Path to local media file to be shared</param>
        [Doc(@"AddMedia")]
        public void AddMedia (string path) => payload.AddMedia(path);
        #endregion

        private readonly ISharePayload payload;
    }
}