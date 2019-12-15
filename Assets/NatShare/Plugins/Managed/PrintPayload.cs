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
    /// EXPERIMENTAL. A payload for printing media
    /// </summary>
    [Doc(@"PrintPayload")]
    public sealed class PrintPayload : ISharePayload {

        #region --Client API--
        /// <summary>
        /// Create a print payload
        /// </summary>
        /// <param name="greyscale">Optional. Should items be printed in greyscale</param>
        /// <param name="landscape">Optional. Should items be printed in landscape orientation</param>
        /// <param name="completionHandler">Optional. Delegate invoked with whether saving was successful</param>
        [Doc(@"PrintPayloadCtor")]
        public PrintPayload (bool greyscale = false, bool landscape = false, Action completionHandler = null) {
            switch (Application.platform) {
                case RuntimePlatform.Android: {
                    var nativePayload = new AndroidJavaObject(@"api.natsuite.natshare.PrintPayload", greyscale, landscape, PayloadAndroid.GetCallbackID(completionHandler));
                    this.payload = new PayloadAndroid(nativePayload);
                    break;
                }
                case RuntimePlatform.IPhonePlayer: {
                    var callback = completionHandler != null ? (IntPtr)GCHandle.Alloc(completionHandler, GCHandleType.Normal) : IntPtr.Zero;
                    var nativePayload = PayloadBridge.CreatePrintPayload(greyscale, landscape, PayloadiOS.OnCompletion, callback);
                    this.payload = new PayloadiOS(nativePayload);
                    break;
                }
                default:
                    Debug.LogError("NatShare Error: PrintPayload is not supported on this platform");
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
        /// Nop. No concept as saving text to the gallery
        /// </summary>
        [Doc(@"AddText")]
        public void AddText (string text) => payload.AddText(text);

        /// <summary>
        /// Add an image to the payload
        /// </summary>
        [Doc(@"AddImage")]
        public void AddImage (Texture2D image) => payload.AddImage(image);

        /// <summary>
        /// Add media to the payload
        /// </summary>
        /// <param name="path">Path to local media file to be shared</param>
        [Doc(@"AddMedia")]
        public void AddMedia (string uri) => payload.AddMedia(uri);
        #endregion

        private readonly ISharePayload payload;
    }
}