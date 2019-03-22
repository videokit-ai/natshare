/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using UnityEngine;
    using System;

    public sealed class SharePayloadiOS : SharePayload {

        #region --SharePayload--

        public SharePayloadiOS (PayloadCommitMode commitMode) {

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

        public override void Commit (Action completionHandler) {

        }

        public override void Dispose () {

        }
        #endregion


        #region --Operations--

        #endregion
    }
}