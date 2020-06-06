/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using UnityEngine;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// Payload for sharing to available social services.
    /// </summary>
    public sealed class SharePayload : ISharePayload {

        #region --Client API--
        /// <summary>
        /// Create a share payload.
        /// </summary>
        public SharePayload () => this.payload = new NativePayload((callback, context) => Bridge.CreateSharePayload(callback, context));

        /// <summary>
        /// Add plain text.
        /// </summary>
        public ISharePayload AddText (string text) {
            payload.AddText(text);
            return this;
        }

        /// <summary>
        /// Add an image to the payload.
        /// Note that the image MUST be readable.
        /// </summary>
        /// <param name="image">Image to be shared.</param>
        public ISharePayload AddImage (Texture2D image) {
            payload.AddImage(image);
            return this;
        }

        /// <summary>
        /// Add a media file to the payload
        /// </summary>
        /// <param name="path">Path to media file to be shared.</param>
        public ISharePayload AddMedia (string path) {
            payload.AddMedia(path);
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