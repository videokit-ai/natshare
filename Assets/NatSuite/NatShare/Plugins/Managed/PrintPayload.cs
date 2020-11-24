/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing {

    using System;
    using Internal;

    /// <summary>
    /// EXPERIMENTAL. Payload for printing media.
    /// </summary>
    public sealed class PrintPayload : NativePayload {

        #region --Client API--
        /// <summary>
        /// Create a print payload.
        /// </summary>
        /// <param name="greyscale">Should items be printed in color. Defaults to `true`.</param>
        /// <param name="landscape">Should items be printed in landscape orientation. Defaults to `false`.</param>
        public PrintPayload (bool color = true, bool landscape = false) : base(Supported ? (IntPtr?)Bridge.CreatePrintPayload(color, landscape) : null) { }
        #endregion
    }
}