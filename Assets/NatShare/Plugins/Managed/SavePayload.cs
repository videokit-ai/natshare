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
    /// A payload for saving to the camera roll
    /// </summary>
    [Doc(@"SavePayload")]
    public sealed class SavePayload : IPayload {

        #region --Client API--
        /// <summary>
        /// Create a save payload
        /// </summary>
        /// <param name="album">Optional. Album name in which contents should be saved</param>
        /// <param name="completionHandler">Optional. Delegate invoked with whether saving was successful</param>
        [Doc(@"SavePayloadCtor")]
        public SavePayload (string album = null, Action completionHandler = null) {
            switch (Application.platform) {
                case RuntimePlatform.Android: {
                    var nativePayload = new AndroidJavaObject(@"com.yusufolokoba.natshare.SavePayload", album, completionHandler);
                    this.payload = new PayloadAndroid(nativePayload);
                    break;
                }
                case RuntimePlatform.IPhonePlayer: {
                    var handlerPtr = completionHandler != null ? (IntPtr)GCHandle.Alloc(completionHandler, GCHandleType.Normal) : IntPtr.Zero;
                    var nativePayload = PayloadBridge.CreateSavePayload(album, PayloadiOS.OnCompletion, handlerPtr);
                    this.payload = new PayloadiOS(nativePayload);
                    break;
                }
                default:
                    Debug.LogError("NatShare Error: SavePayload is not supported on this platform");
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

        /// <summary>
        /// Commit the payload
        /// </summary>
        [Doc(@"Commit")]
        public void Commit () {
            payload.Commit();
        }
        #endregion

        private readonly IPayload payload;
    }
}