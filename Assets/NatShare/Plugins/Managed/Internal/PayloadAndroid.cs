/* 
*   NatShare
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatShare.Internal {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;
    using System.Runtime.InteropServices;

    public sealed class PayloadAndroid : ISharePayload {

        #region --IPayload--

        public PayloadAndroid (AndroidJavaObject payload) => this.payload = payload;

        public void Dispose () => payload.Call(@"commit");

        public void AddText (string text) => payload.Call(@"addText", text);

        public void AddImage (Texture2D image) => payload.Call(@"addImage", image.EncodeToPNG());

        public void AddMedia (string uri) => payload.Call(@"addMedia", uri);
        #endregion
        

        #region --Operations--
        
        private readonly AndroidJavaObject payload;
        private static CallbackManager instance;

        public static int GetCallbackID (Action callback) {
            // Give Java the C# delegate to invoke
            if (instance == null) {
                instance = new CallbackManager();
                using (var Bridge = new AndroidJavaClass(@"api.natsuite.natshare.Bridge"))
                    Bridge.CallStatic(@"setCallback", instance);
            }
            // Get handle
            return callback != null ? (int)(IntPtr)GCHandle.Alloc(callback, GCHandleType.Normal) : 0;
        }

        private class CallbackManager : AndroidJavaProxy {
            
            public CallbackManager () : base(@"api.natsuite.natshare.Bridge$CompletionHandler") { }

            [Preserve]
            private static void onCompletion (int context) {
                var handle = (GCHandle)(IntPtr)context;
                var handler = handle.Target as Action;
                handle.Free();
                handler();
            }
        }
        #endregion
    }
}