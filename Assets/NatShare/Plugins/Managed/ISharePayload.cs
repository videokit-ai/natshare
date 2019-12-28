/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba.
*/

namespace NatShare {

    using UnityEngine;
    using System.Threading.Tasks;

    /// <summary>
    /// A payload for social sharing
    /// </summary>
    public interface ISharePayload {
        
        /// <summary>
        /// Add text to the payload
        /// </summary>
        void AddText (string text);

        /// <summary>
        /// Add an image to the payload.
        /// </summary>
        /// <param name="image">Image</param>
        void AddImage (Texture2D image);

        /// <summary>
        /// Add media to the payload
        /// </summary>
        /// <param name="path">Path to local media file to be shared</param>
        void AddMedia (string path);

        /// <summary>
        /// Commit the payload and return whether payload was successfully shared
        /// </summary>
        Task<bool> Commit ();
    }
}