/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using UnityEngine;
    using System;

    public sealed class PayloadAndroid : ISharePayload {

        #region --IPayload--

        public PayloadAndroid (AndroidJavaObject payload) {
            this.payload = payload;
        }

        public void Dispose () {
            payload.Call(@"commit");
        }

        public void AddText (string text) {
            payload.Call(@"addText", text);
        }

        public void AddImage (Texture2D image) {
            payload.Call(@"addImage", image.EncodeToPNG());
        }

        public void AddMedia (string uri) {
            payload.Call(@"addMedia", uri);
        }
        #endregion
        
        private readonly AndroidJavaObject payload;
    }
}