/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Sharing.Internal {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public abstract class NativePayload : ISharePayload {

        #region --Client API--
        /// <summary>
        /// Add text to the payload.
        /// </summary>
        /// <param name="text">Plain text to add.</param>
        public virtual void AddText (string text) => payload?.AddText(text);

        /// <summary>
        /// Add an image to the payload from a pixel buffer.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer containing image to add.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        public virtual void AddImage<T> (T[] pixelBuffer, int width, int height) where T : struct {
            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            var frameBuffer = new Texture2D(width, height, TextureFormat.RGBA32, false);
            frameBuffer.LoadRawTextureData(handle.AddrOfPinnedObject(), width * height * 4);
            handle.Free();
            AddImage(frameBuffer);
            Texture2D.Destroy(frameBuffer);
        }

        /// <summary>
        /// Add an image to the payload.
        /// Note that the image MUST be readable.
        /// </summary>
        /// <param name="image">Image to add.</param>
        public virtual void AddImage (Texture2D image) {
            if (image.isReadable) {
                var jpegData = ImageConversion.EncodeToJPG(image); // Faster than PNG #85
                payload?.AddImage(jpegData, jpegData.Length);
            }
            else
                Debug.LogError("NatShare Error: Cannot add non-readable texture to payload");
        }

        /// <summary>
        /// Add a media file to the payload.
        /// </summary>
        /// <param name="path">Path to media file to add.</param>
        public virtual void AddMedia (string uri) => payload?.AddMedia(uri);

        /// <summary>
        /// Commit the payload.
        /// </summary>
        /// <returns>Whether the sharing action was successfully completed.</returns>
        public virtual Task<bool> Commit () {
            // Check
            if (payload == null)
                return Task.FromResult(false);
            // Commit
            var commitTask = new TaskCompletionSource<bool>();
            var handle = GCHandle.Alloc(commitTask, GCHandleType.Normal);
            payload?.Commit(OnCompletion, (IntPtr)handle);
            return commitTask.Task;
        }
        #endregion


        #region --Operations--

        private readonly IntPtr? payload;
        protected static bool Supported => Application.platform == RuntimePlatform.IPhonePlayer || Application.platform ==  RuntimePlatform.Android;

        protected NativePayload (IntPtr? payload) => this.payload = payload;

        [MonoPInvokeCallback(typeof(Bridge.CompletionHandler))]
        private static void OnCompletion (IntPtr context, bool success) {
            // Get task
            var handle = (GCHandle)context;
            if (!handle.IsAllocated) // iOS can invoke this callback more than once, so be cautious :/
                return;
            // Complete task
            var commitTask = handle.Target as TaskCompletionSource<bool>;
            handle.Free();
            commitTask.SetResult(success);
        }
        #endregion
    }
}