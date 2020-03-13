/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using UnityEngine;
    using System.Threading.Tasks;
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
        [Doc(@"PrintPayloadCtor")]
        public PrintPayload (bool greyscale = false, bool landscape = false) {
            switch (Application.platform) {
                case RuntimePlatform.Android: this.payload = new AndroidPayload(callback => new AndroidJavaObject(@"api.natsuite.natshare.PrintPayload", greyscale, landscape, callback)); break;
                case RuntimePlatform.IPhonePlayer: this.payload = new NativePayload((callback, context) => Bridge.CreatePrintPayload(greyscale, landscape, callback, context)); break;
                default: Debug.LogError("NatShare Error: PrintPayload is not supported on this platform"); break;
            }
        }

        /// <summary>
        /// Nop. No concept as saving text to the gallery
        /// </summary>
        [Doc(@"AddText")]
        public void AddText (string text) => payload?.AddText(text);

        /// <summary>
        /// Add an image to the payload
        /// </summary>
        [Doc(@"AddImage")]
        public void AddImage (Texture2D image) => payload?.AddImage(image);

        /// <summary>
        /// Add media to the payload
        /// </summary>
        /// <param name="path">Path to local media file to be shared</param>
        [Doc(@"AddMedia")]
        public void AddMedia (string uri) => payload?.AddMedia(uri);

        /// <summary>
        /// Commit the payload and return whether payload was successfully shared
        /// </summary>
        [Doc(@"Commit")]
        public Task<bool> Commit () => payload?.Commit();
        #endregion

        private readonly ISharePayload payload;
    }
}