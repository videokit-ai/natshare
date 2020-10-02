/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using UnityEngine;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// EXPERIMENTAL. Payload for printing media.
    /// </summary>
    public sealed class PrintPayload : ISharePayload {

        #region --Client API--
        /// <summary>
        /// Create a print payload.
        /// </summary>
        /// <param name="greyscale">Should items be printed in color. Defaults to `true`.</param>
        /// <param name="landscape">Should items be printed in landscape orientation. Defaults to `false`.</param>
        public PrintPayload (bool color = true, bool landscape = false) => this.payload = new NativePayload((callback, context) => Bridge.CreatePrintPayload(color, landscape, callback, context));

        /// <summary>
        /// Add text to the payload.
        /// </summary>
        public ISharePayload AddText (string text) {
            payload.AddText(text);
            return this;
        }

        /// <summary>
        /// Add an image to the payload.
        /// Note that the image MUST be readable.
        /// </summary>
        /// <param name="image">Image to be printed.</param>
        public ISharePayload AddImage (Texture2D image) {
            payload.AddImage(image);
            return this;
        }

        /// <summary>
        /// Add an image file to the payload.
        /// </summary>
        /// <param name="path">Path to image file to be printed.</param>
        public ISharePayload AddMedia (string uri) {
            payload.AddMedia(uri);
            return this;
        }

        /// <summary>
        /// Commit the payload and return the success value.
        /// </summary>
        public Task<bool> Commit () => payload.Commit();
        #endregion

        private readonly ISharePayload payload;
    }
}