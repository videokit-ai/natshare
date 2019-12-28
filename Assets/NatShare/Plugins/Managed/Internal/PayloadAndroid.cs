/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;
    using System.Threading.Tasks;

    public sealed class PayloadAndroid : AndroidJavaProxy, ISharePayload {

        #region --IPayload--

        public PayloadAndroid (Func<AndroidJavaProxy, AndroidJavaObject> payloadCreator) : base(@"api.natsuite.natshare.Payload$CompletionHandler") {
            this.payload = payloadCreator(this);
            this.commitTask = new TaskCompletionSource<bool>();
        }

        public void AddText (string text) => payload.Call(@"addText", text);

        public void AddImage (Texture2D image) => payload.Call(@"addImage", image.EncodeToPNG());

        public void AddMedia (string uri) => payload.Call(@"addMedia", uri);

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