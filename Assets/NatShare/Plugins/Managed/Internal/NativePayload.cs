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

    public sealed class NativePayload : ISharePayload {

        #region --IPayload--

        public NativePayload (Func<Bridge.CompletionHandler, IntPtr, IntPtr> payloadCreator) {
            this.commitTask = new TaskCompletionSource<bool>();
            var handle = GCHandle.Alloc(commitTask, GCHandleType.Normal);
            this.payload = payloadCreator(OnCompletion, (IntPtr)handle);
        }

        public void AddText (string text) => payload.AddText(text);

        public void AddImage (Texture2D image) {
            if (!image.isReadable) {
                Debug.LogError("NatShare Error: Cannot add non-readable texture to payload");
                return;
            }
            var pixels = image.GetPixels32(); // This ensures that pixel buffer sent to native is always RGBA32
            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            payload.AddImage(handle.AddrOfPinnedObject(), image.width, image.height);
            handle.Free();
        }

        public void AddMedia (string uri) => payload.AddMedia(uri);

        public Task<bool> Commit () {
            payload.Commit();
            return commitTask.Task;
        }
        #endregion


        #region --Operations--

        private readonly IntPtr payload;
        private readonly TaskCompletionSource<bool> commitTask;

        [MonoPInvokeCallback(typeof(Bridge.CompletionHandler))]
        private static void OnCompletion (IntPtr context, bool success) {
            var handle = (GCHandle)context;
            var commitTask = handle.Target as TaskCompletionSource<bool>;
            handle.Free();
            commitTask.SetResult(success);
        }
        #endregion
    }
}