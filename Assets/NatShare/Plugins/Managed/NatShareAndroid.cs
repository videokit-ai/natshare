/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

	using UnityEngine;
	using System;

	public class NatShareAndroid : AndroidJavaProxy, INatShare {

		private ShareCallback callback;
		private readonly AndroidJavaObject natshare;

		public NatShareAndroid () : base("com.yusufolokoba.natshare.NatShareDelegate") {
			natshare = new AndroidJavaObject("com.yusufolokoba.natshare.NatShare", this);
		}

		bool INatShare.ShareText (string text, ShareCallback callback) {
			this.callback = callback;
			return natshare.Call<bool>("shareText", text);
		}

		bool INatShare.ShareImage (byte[] pngData, string message, ShareCallback callback) {
			this.callback = callback;
			return natshare.Call<bool>("shareImage", pngData, message);
		}

		bool INatShare.ShareMedia (string path, string message, ShareCallback callback) {
			this.callback = callback;
			return natshare.Call<bool>("shareMedia", path, message);
		}

		bool INatShare.SaveToCameraRoll (byte[] pngData) {
			return natshare.Call<bool>("saveImageToCameraRoll", pngData);
		}

		bool INatShare.SaveToCameraRoll (string path) {
			return natshare.Call<bool>("saveMediaToCameraRoll", path);
		}

		void INatShare.GetThumbnail (string videoPath, Action<Texture2D> callback, float time) {
			var thumbnail = natshare.Call<AndroidJavaObject>("getThumbnail", videoPath, time);
            if (!thumbnail.Call<bool>("isLoaded")) {
                Debug.LogError("NatShare Error: Failed to get thumbnail for video at path: "+videoPath);
                return;
            }
            var pixelData = AndroidJNI.FromByteArray(thumbnail
                .Get<AndroidJavaObject>("pixelBuffer")
                .Call<AndroidJavaObject>("array")
                .GetRawObject()
            );
            var image = new Texture2D(
                thumbnail.Get<int>("width"),
                thumbnail.Get<int>("height"),
                TextureFormat.RGB565,
                false
            );
            image.LoadRawTextureData(pixelData);
            image.Apply();
            callback(image);
		}

		void onShare (bool completed) {
			/**
			 * Unfortunately, we can't rely on the `completed` value as a number of sharing receivers
			 * don't correctly set this value, so the sharing activity will report as not completed 
			 * even though the user did in fact complete the sharing activity
			 */
			if (callback != null) callback();
		}
	}
}