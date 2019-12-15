/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba.
*/

namespace NatShare.Internal {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;

    public sealed class PayloadiOS : ISharePayload {

        #region --IPayload--

        public PayloadiOS (IntPtr payload) => this.payload = payload;

        public void Dispose () => payload.Commit();

        public void AddText (string text) => payload.AddText(text);

        public void AddImage (Texture2D image) {
            var pixels = image.GetPixels32(); // This ensures that pixel buffer sent to native is always RGBA32
            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            payload.AddImage(handle.AddrOfPinnedObject(), image.width, image.height);
            handle.Free();
        }

        public void AddMedia (string uri) => payload.AddMedia(uri);
        #endregion


        #region --Operations--

        private readonly IntPtr payload;

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        public static void OnCompletion (IntPtr context) {
            var handle = (GCHandle)context;
            var handler = handle.Target as Action;
            handle.Free();
            handler();
        }
        #endregion
    }
}