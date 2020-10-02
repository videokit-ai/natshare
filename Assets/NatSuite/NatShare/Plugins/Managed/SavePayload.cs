/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using UnityEngine;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// Payload for saving to the camera roll.
    /// </summary>
    public sealed class SavePayload : ISharePayload {

        #region --Client API--
        /// <summary>
        /// Create a save payload.
        /// </summary>
        /// <param name="album">Optional. Album name in which contents should be saved.</param>
        public SavePayload (string album = default) => this.payload = new NativePayload((callback, context) => Bridge.CreateSavePayload(album, callback, context));

        /// <summary>
        /// Nop. No concept as saving text to the camera roll.
        /// </summary>
        public ISharePayload AddText (string text) {
            payload.AddText(text);
            return this;
        }

        /// <summary>
        /// Add an image to the payload.
        /// Note that the image MUST be readable.
        /// </summary>
        /// <param name="image">Image to be saved.</param>
        public ISharePayload AddImage (Texture2D image) {
            payload.AddImage(image);
            return this;
        }

        /// <summary>
        /// Add a media file to the payload.
        /// </summary>
        /// <param name="path">Path to media file to be saved.</param>
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