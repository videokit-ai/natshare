/* 
*   NatShare
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Sharing {

    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using UnityEngine;
    using AOT;
    using Internal;

    /// <summary>
    /// Payload for saving media to the camera roll.
    /// </summary>
    public sealed class SavePayload : IDisposable {

        #region --Client API--
        /// <summary>
        /// Album name in which media will be saved.
        /// </summary>
        public readonly string album;

        /// <summary>
        /// Whether the save payload is supported on this platform.
        /// </summary>
        public static bool Supported => Array.IndexOf(
            new [] { RuntimePlatform.Android, RuntimePlatform.IPhonePlayer, RuntimePlatform.WebGLPlayer },
            Application.platform
        ) >= 0;

        /// <summary>
        /// Create a save payload.
        /// </summary>
        /// <param name="album">Optional. Album name in which media should be saved.</param>
        public SavePayload (string album = default) {
            this.album = album;
            if (Supported)
                NatShare.CreateSavePayload(album, out payload);
            else
                Debug.LogWarning($"SavePayload is not supported on {Application.platform}");
        }

        /// <summary>
        /// Add an image to the payload.
        /// </summary>
        /// <param name="image">Image to save.</param>
        public void AddImage (Texture2D image) {
            // Check
            if (payload == IntPtr.Zero)
                return;
            // Check image
            if (!image) {
                Debug.LogError("SavePayload cannot add null or invalid texture");
                return;
            }
            // Check readability
            if (!image.isReadable) {
                Debug.LogError("SavePayload cannot add non-readable texture");
                return;
            }
            // Add
            var imageBuffer = ImageConversion.EncodeToPNG(image);
            payload.AddSavePayloadImage(imageBuffer, imageBuffer.Length);
        }

        /// <summary>
        /// Add a media file to the payload.
        /// </summary>
        /// <param name="path">Path to media file to save.</param>
        public void AddMedia (string path) {
            // Check
            if (payload == IntPtr.Zero)
                return;
            // Add
            payload.AddSavePayloadMedia(path);
        }

        /// <summary>
        /// Save the payload and return the success value.
        /// </summary>
        /// <returns>Whether the save action was successfully completed.</returns>
        public Task<bool> Save () {
            // Check
            if (payload == IntPtr.Zero)
                return Task.FromResult(false);
            // Commit
            var saveTask = new TaskCompletionSource<bool>();
            var handle = GCHandle.Alloc(saveTask, GCHandleType.Normal);
            payload.Save(OnSave, (IntPtr)handle);
            return saveTask.Task;
        }

        /// <summary>
        /// Dispose the payload and release resources.
        /// </summary>
        public void Dispose () => payload.ReleaseSavePayload();

        /// <summary>
        /// Request permissions to save media to the camera roll.
        /// </summary>
        public static Task<bool> RequestPermissions () {
            // Check
            if (!Supported)
                return Task.FromResult(false);
            // Request
            var tcs = new TaskCompletionSource<bool>();
            var handle = GCHandle.Alloc(tcs, GCHandleType.Normal);
            NatShare.RequestPermissions(OnPermissionResult, (IntPtr)handle);
            return tcs.Task;
        }
        #endregion


        #region --Operations--
        private readonly IntPtr payload;

        [MonoPInvokeCallback(typeof(NatShare.SaveResultHandler))]
        private static void OnSave (IntPtr context, bool success) {
            var handle = (GCHandle)context;
            var saveTask = handle.Target as TaskCompletionSource<bool>;
            handle.Free();
            saveTask?.SetResult(success);
        }

        [MonoPInvokeCallback(typeof(NatShare.SavePermissionHandler))]
        private static void OnPermissionResult (IntPtr context, bool granted) {
            var handle = (GCHandle)context;
            var handler = handle.Target as TaskCompletionSource<bool>;
            handle.Free();
            handler?.SetResult(granted);
        }
        #endregion


        #region --DEPRECATED--
        [Obsolete("Deprecated in NatShare 1.3. Use the `Save` method instead.", false)]
        public Task<bool> Commit () => Save();
        #endregion
    }
}