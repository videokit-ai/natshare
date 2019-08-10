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
    /// EXPERIMENTAL. A payload for printing media
    /// </summary>
    [Doc(@"PrintPayload")]
    public sealed class PrintPayload : IPayload { // EXPERIMENTAL

        #region --Client API--
        /// <summary>
        /// Create a print payload
        /// </summary>
        /// <param name="greyscale">Optional. Should items be printed in greyscale</param>
        /// <param name="landscape">Optional. Should items be printed in landscape orientation</param>
        /// <param name="completionHandler">Optional. Delegate invoked with whether saving was successful</param>
        [Doc(@"PrintPayloadCtor")]
        public PrintPayload (bool greyscale = false, bool landscape = false, Action completionHandler = null) { // INCOMPLETE
            switch (Application.platform) {
                case RuntimePlatform.Android: {
                    var nativePayload = new AndroidJavaObject(@"com.yusufolokoba.natshare.PrintPayload", greyscale, landscape, completionHandler);
                    this.payload = new PayloadAndroid(nativePayload);
                    break;
                }
                case RuntimePlatform.IPhonePlayer: {
                    var handlerPtr = completionHandler != null ? (IntPtr)GCHandle.Alloc(completionHandler, GCHandleType.Normal) : IntPtr.Zero;
                    var nativePayload = PayloadBridge.CreatePrintPayload(greyscale, landscape, PayloadiOS.OnCompletion, handlerPtr);
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
        public void Dispose () {
            payload.Dispose();
        }

        /// <summary>
        /// Nop. No concept as saving text to the gallery
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
        /// Add media with at a given URI
        /// </summary>
        [Doc(@"AddMedia")]
        public void AddMedia (string uri) {
            payload.AddMedia(uri);
        }
        #endregion

        private readonly IPayload payload;
    }
}