/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
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
    public sealed class SharePayload : IPayload {

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
                    var nativePayload = new AndroidJavaObject(@"com.yusufolokoba.natshare.SharePayload", subject ?? "", new AndroidJavaRunnable(completionHandler));
                    this.payload = new PayloadAndroid(nativePayload);
                    break;
                }
                case RuntimePlatform.IPhonePlayer: {
                    var handlerPtr = completionHandler != null ? (IntPtr)GCHandle.Alloc(completionHandler, GCHandleType.Normal) : IntPtr.Zero;
                    var nativePayload = PayloadBridge.CreateSharePayload(subject, PayloadiOS.OnCompletion, handlerPtr);
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
        public void Dispose () {
            payload.Dispose();
        }

        /// <summary>
        /// Add plain text
        /// </summary>
        [Doc(@"AddText")]
        public void AddText (string text) {
            payload.AddText(text);
        }

        /// <summary>
        /// Add an image to the payload.
        /// The pixel buffer MUST in the RGBA32 format.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [Doc(@"AddImage")]
        public void AddImage (byte[] pixelBuffer, int width, int height) {
            payload.AddImage(pixelBuffer, width, height);
        }

        /// <summary>
        /// Add an image to the payload.
        /// The texture MUST in the RGBA32 format.
        /// </summary>
        /// <param name="image">Image to save</param>
        [Doc(@"SharePayloadAddImageTexture")]
        public void AddImage (Texture2D image) {
            AddImage(image.GetRawTextureData(), image.width, image.height);
        }

        /// <summary>
        /// Add media to the payload
        /// </summary>
        /// <param name="path">Path to local media file to be shared</param>
        [Doc(@"AddMedia")]
        public void AddMedia (string path) {
            payload.AddMedia(path);
        }
        #endregion

        private readonly IPayload payload;
    }
}