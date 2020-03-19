/* 
*   NatShare
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Sharing.Internal {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;
    using System.Threading.Tasks;

    public sealed class AndroidPayload : AndroidJavaProxy, ISharePayload {

        #region --IPayload--

        public AndroidPayload (Func<AndroidJavaProxy, AndroidJavaObject> payloadCreator) : base(@"api.natsuite.natshare.Payload$CompletionHandler") {
            this.payload = payloadCreator(this);
            this.commitTask = new TaskCompletionSource<bool>();
        }

        public ISharePayload AddText (string text) {
            payload.Call(@"addText", text);
            return this;
        }

        public ISharePayload AddImage (Texture2D image) {
            if (image.isReadable)
                payload.Call(@"addImage", ImageConversion.EncodeToPNG(image));
            else
                Debug.LogError("NatShare Error: Cannot add non-readable texture to payload");            
            return this;
        }

        public ISharePayload AddMedia (string uri) {
            payload.Call(@"addMedia", uri);
            return this;
        }

        public Task<bool> Commit () {
            payload.Call(@"commit");
            return commitTask.Task;
        }
        #endregion
        

        #region --Operations--
        private readonly AndroidJavaObject payload;
        private readonly TaskCompletionSource<bool> commitTask;
        [Preserve] private void onCompletion (bool success) => commitTask.SetResult(success);
        #endregion
    }
}