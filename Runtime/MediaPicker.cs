/* 
*   NatShare
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Sharing {

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// </summary>
    internal sealed class MediaPicker : IDisposable { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// </summary>
        [Flags]
        public enum MediaType {
            /// <summary>
            /// Unknown media type.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Image file.
            /// </summary>
            Image   = 1 << 0,
            /// <summary>
            /// Video file.
            /// </summary>
            Video   = 1 << 1,
            /// <summary>
            /// Audio file.
            /// </summary>
            Audio   = 1 << 2,
        }

        /// <summary>
        /// Get or set the preferred media type.
        /// This will update the `mimeTypes` field depending on the specified media type.
        /// </summary>
        public MediaType mediaType {
            get => preferredType;
            set => mimeTypes = ToMIMETypeArray(preferredType = value);
        }

        /// <summary>
        /// List of allowed media MIME types to pick.
        /// This field directly controls what type of media the user can select.
        /// </summary>
        public string[] mimeTypes;

        /// <summary>
        /// Maximum number of items the user can pick.
        /// </summary>
        public int maxPicks;

        /// <summary>
        /// Whether the media picker is supported on this platform.
        /// </summary>
        public static bool Supported => Array.IndexOf(
            new [] { RuntimePlatform.Android, RuntimePlatform.IPhonePlayer, RuntimePlatform.WebGLPlayer },
            Application.platform
        ) >= 0;

        /// <summary>
        /// Create a media picker.
        /// </summary>
        public MediaPicker () {
            this.mediaType = MediaType.Image; // this implicitly sets `mimeTypes`
            this.maxPicks = 1;
            if (Supported)
                NatShare.CreateMediaPicker(out picker);
            else
                Debug.LogWarning($"MediaPicker is not supported on {Application.platform}");
        }

        /// <summary>
        /// Present the system media picker to the user.
        /// </summary>
        /// <returns>Picked media URLs.</returns>
        public Task<string[]> Pick () {
            // Check
            if (picker == IntPtr.Zero)
                return Task.FromResult(new string[0]);
            // Pick
            picker.SetMIMETypes(mimeTypes, mimeTypes.Length);
            picker.SetMaxPicks(maxPicks);
            return default;
        }

        /// <summary>
        /// Dispose the picker and release resources.
        /// </summary>
        public void Dispose () => picker.ReleaseMediaPicker();
        #endregion


        #region --Operations--
        private readonly IntPtr picker;
        private MediaType preferredType;

        private static string[] ToMIMETypeArray (MediaType type) {
            var result = new List<string>();
            if (type.HasFlag(MediaType.Image))
                result.Add("image/*");
            if (type.HasFlag(MediaType.Video))
                result.Add("video/*");
            if (type.HasFlag(MediaType.Audio))
                result.Add("audio/*");
            return result.ToArray();
        }
        #endregion
    }
}