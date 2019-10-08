/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;

    public sealed class PayloadiOS : ISharePayload {

        #region --IPayload--

        public PayloadiOS (IntPtr payload) {
            this.payload = payload;
        }

        public void Dispose () {
            payload.Commit();
        }

        public void AddText (string text) {
            payload.AddText(text);
        }

        public void AddImage (Texture2D image) {
            payload.AddImage(image.GetRawTextureData(), image.width, image.height);
        }

        public void AddMedia (string uri) {
            payload.AddMedia(uri);
        }
        #endregion


        #region --Operations--

        private readonly IntPtr payload;

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        public static void OnCompletion (IntPtr context) {
            if (context == IntPtr.Zero)
                return;
            var callbackHandle = (GCHandle)context;
            var callback = callbackHandle.Target as Action;
            callbackHandle.Free();
            callback();
        }
        #endregion
    }
}