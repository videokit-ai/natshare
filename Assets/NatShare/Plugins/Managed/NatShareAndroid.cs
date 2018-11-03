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

		bool INatShare.Share (byte[] pngData, ShareCallback callback) {
			this.callback = callback;
			return natshare.Call<bool>("shareImage", pngData);
		}

		bool INatShare.Share (string media, ShareCallback callback) {
			this.callback = callback;
			Uri uri;
			if (Uri.TryCreate(media, UriKind.Absolute, out uri))
				return natshare.Call<bool>("shareMedia", media);
			else
				return natshare.Call<bool>("shareText", media);
		}

		bool INatShare.SaveToCameraRoll (byte[] pngData, string album) {
			return natshare.Call<bool>("saveImageToCameraRoll", pngData, album);
		}

		bool INatShare.SaveToCameraRoll (string path, string album, bool copy) {
			return natshare.Call<bool>("saveMediaToCameraRoll", path, album, copy);
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