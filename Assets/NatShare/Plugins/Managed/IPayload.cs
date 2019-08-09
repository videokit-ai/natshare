/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare {

    using System;

    /// <summary>
    /// A payload for social sharing
    /// </summary>
    public interface IPayload : IDisposable {
        /// <summary>
        /// Add text to the payload
        /// </summary>
        void AddText (string text);
        /// <summary>
        /// Add an image to the payload.
        /// The pixel buffer MUST in the RGBA32 format.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void AddImage (byte[] pixelBuffer, int width, int height); // Parity with NatCorder and NatExtractor :)
        /// <summary>
        /// Add media to the payload
        /// </summary>
        /// <param name="path">Path to media to be shared. MUST be prepended with URI scheme</param>
        void AddMedia (string path);
    }
}