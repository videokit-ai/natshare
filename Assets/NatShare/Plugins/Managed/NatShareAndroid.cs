/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Platforms {

	using UnityEngine;

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

		Texture2D INatShare.GetThumbnail (string videoPath, float time) {
			using (var thumbnail = natshare.Call<AndroidJavaObject>("getThumbnail", videoPath, time)) {
				if (!thumbnail.Call<bool>("isLoaded")) {
					Debug.LogError("NatShare Error: Failed to get thumbnail for video at path: "+videoPath);
					return null;
				}
				var width = thumbnail.Get<int>("width");
				var height = thumbnail.Get<int>("height");
				using (var pixelBuffer = thumbnail.Get<AndroidJavaObject>("pixelBuffer")) 
					using (var array = pixelBuffer.Call<AndroidJavaObject>("array")) {
						var pixelData = AndroidJNI.FromByteArray(array.GetRawObject());
						var image = new Texture2D(width, height, TextureFormat.RGB565, false); // Weird texture format IMO
						image.LoadRawTextureData(pixelData);
						image.Apply();
						return image;
				}
			}
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