/* 
*   NatShare
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(@"NatML.Sharing.Editor")]
namespace NatML.Sharing {

    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using UnityEngine;
    using AOT;
    using Internal;

    /// <summary>
    /// Payload for sharing media to available social services.
    /// </summary>
    public sealed class SharePayload : IDisposable {

        #region --Client API--
        /// <summary>
        /// Whether the share payload is supported on this platform.
        /// </summary>
        public static bool Supported => Array.IndexOf(
            new [] { RuntimePlatform.Android, RuntimePlatform.IPhonePlayer },
            Application.platform
        ) >= 0;

        /// <summary>
        /// Create a share payload.
        /// </summary>
        public SharePayload () {
            if (Supported)
                NatShare.CreateSharePayload(out payload);
            else
                Debug.LogWarning($"SharePayload is not supported on {Application.platform}");
        }

        /// <summary>
        /// Add text to the payload.
        /// </summary>
        /// <param name="text">Plain text to add.</param>
        public void AddText (string text) {
            // Check
            if (payload == IntPtr.Zero)
                return;
            // Add
            payload.AddSharePayloadText(text);
        }

        /// <summary>
        /// Add an image to the payload.
        /// </summary>
        /// <param name="image">Image to add.</param>
        public void AddImage (Texture2D image) {
            // Check
            if (payload == IntPtr.Zero)
                return;
            // Check image
            if (!image) {
                Debug.LogError("SharePayload cannot add null or invalid texture");
                return;
            }
            // Check readability
            if (!image.isReadable) {
                Debug.LogError("SharePayload cannot add non-readable texture");
                return;
            }
            // Add
            var imageBuffer = ImageConversion.EncodeToPNG(image);
            payload.AddSharePayloadImage(imageBuffer, imageBuffer.Length);
        }

        /// <summary>
        /// Add a media file to the payload.
        /// </summary>
        /// <param name="path">Path to media file to add.</param>
        public void AddMedia (string path) {
            // Check
            if (payload == IntPtr.Zero)
                return;
            // Add
            payload.AddSharePayloadMedia(path);
        }

        /// <summary>
        /// Share the payload and return the success value.
        /// </summary>
        /// <returns>Identifier of the app chosen by the user, or `null` if sharing action was canceled.</returns>
        public Task<string> Share () {
            // Check
            if (payload == IntPtr.Zero)
                return Task.FromResult(null as string);
            // Commit
            var shareTask = new TaskCompletionSource<string>();
            var handle = GCHandle.Alloc(shareTask, GCHandleType.Normal);
            payload.Share(OnShare, (IntPtr)handle);
            return shareTask.Task;
        }

        /// <summary>
        /// Dispose the payload and release resources.
        /// </summary>
        public void Dispose () => payload.ReleaseSharePayload();
        #endregion


        #region --Operations--
        private readonly IntPtr payload;

        [MonoPInvokeCallback(typeof(NatShare.ShareResultHandler))]
        private static void OnShare (IntPtr context, IntPtr receiver) {
            var handle = (GCHandle)context;
            var shareTask = handle.Target as TaskCompletionSource<string>;
            handle.Free();
            shareTask?.SetResult(Marshal.PtrToStringAuto(receiver));
        }
        #endregion


        #region --DEPRECATED--
        [Obsolete("Deprecated in NatShare 1.3. Use the `Share` method instead.", false)]
        public async Task<bool> Commit () => await Share() != null;
        #endregion
    }
}