/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using UnityEngine;
    using System;

    public sealed class SharePayloadAndroid : SharePayload {

        #region --SharePayload--

        public SharePayloadAndroid () {

        }

        public override SharePayload SetSubject (string subject) {

            return this;
        }

        public override SharePayload AddText (string text) {

            return this;
        }

        public override SharePayload AddImage (Texture2D image) {
            
            return this;
        }

        public override SharePayload AddMedia (string uri) {

            return this;
        }

        public override void Share (Action<bool> completionHandler = null) {

        }

        public override void Save (string album = null, Action<bool> completionHandler = null) {

        }

        public override void Dispose () {

        }
        #endregion


        #region --Operations--

        #endregion
    }
}