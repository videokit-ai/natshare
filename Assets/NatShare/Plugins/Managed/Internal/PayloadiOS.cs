/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using AOT;
    using System;
    using System.Runtime.InteropServices;

    public sealed class PayloadiOS : IPayload {

        #region --IPayload--

        public PayloadiOS (IntPtr payload) {
            this.payload = payload;
        }

        public void Dispose () {
            payload.Dispose();
        }

        public void AddText (string text) {
            payload.AddText(text);
        }

        public void AddImage (byte[] pixelBuffer, int width, int height) {
            payload.AddImage(pixelBuffer, width, height);
        }

        public void AddMedia (string uri) {
            payload.AddMedia(uri);
        }

        public void Commit () {
            payload.Commit();
        }
        #endregion


        #region --Operations--

        private readonly IntPtr payload;

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        public static void OnCompletion (IntPtr context) {
            var callbackHandle = (GCHandle)context;
            var callback = callbackHandle.Target as Action;
            callbackHandle.Free();
            callback();
        }
        #endregion
    }
}