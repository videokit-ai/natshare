/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using UnityEngine;
    using System;

    public sealed class PayloadAndroid : IPayload {

        #region --IPayload--

        public PayloadAndroid (AndroidJavaObject payload) {
            this.payload = payload;
        }

        public void Dispose () {
            payload.Call(@"dispose");
        }

        public void AddText (string text) {
            payload.Call(@"addText", text);
        }

        public void AddImage (byte[] pixelBuffer, int width, int height) {
            payload.Call(@"addImage", pixelBuffer, width, height);
        }

        public void AddMedia (string uri) {
            payload.Call(@"addMedia", uri);
        }

        public void Commit () {
            payload.Call(@"commit");
        }
        #endregion


        #region --Operations--
        
        private readonly AndroidJavaObject payload;
        #endregion
    }
}