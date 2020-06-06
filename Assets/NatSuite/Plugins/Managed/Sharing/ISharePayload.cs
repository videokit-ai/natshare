/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using UnityEngine;
    using System.Threading.Tasks;

    /// <summary>
    /// A payload for social sharing.
    /// </summary>
    public interface ISharePayload {
        
        /// <summary>
        /// Add text to the payload.
        /// </summary>
        ISharePayload AddText (string text);

        /// <summary>
        /// Add an image to the payload.
        /// </summary>
        /// <param name="image">Image to the shared.</param>
        ISharePayload AddImage (Texture2D image);

        /// <summary>
        /// Add a media file to the payload.
        /// </summary>
        /// <param name="path">Path to media file to be shared.</param>
        ISharePayload AddMedia (string path);

        /// <summary>
        /// Commit the payload and return the success value.
        /// </summary>
        Task<bool> Commit ();
    }
}